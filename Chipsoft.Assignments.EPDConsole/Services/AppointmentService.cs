using Chipsoft.Assignments.EPDConsole.Interfaces;
using Chipsoft.Assignments.EPDConsole.Models;

namespace Chipsoft.Assignments.EPDConsole.Services;

public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientService _patientService;
        private readonly IPhysicianService _physicianService;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientService patientService,
            IPhysicianService physicianService)
        {
            _appointmentRepository = appointmentRepository;
            _patientService = patientService;
            _physicianService = physicianService;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepository.GetAppointmentsWithDetailsAsync();
        }

        public async Task<Appointment> AddAppointmentAsync(Appointment appointment)
        {
            if (appointment.DateTime < DateTime.Now) throw new ArgumentException("Afspraak kan niet in het verleden zijn");

            if (!await _patientService.PatientExistsAsync(appointment.PatientId)) throw new ArgumentException("Patiënt bestaat niet");

            if (!await _physicianService.PhysicianExistsAsync(appointment.PhysicianId)) throw new ArgumentException("Arts bestaat niet");

            if (!await IsTimeSlotAvailableAsync(appointment.DateTime, appointment.PhysicianId)) throw new ArgumentException("Tijdslot is niet beschikbaar voor deze arts");

            return await _appointmentRepository.AddAsync(appointment);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId)
        {
            return await _appointmentRepository.GetAppointmentsByPatientAsync(patientId);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPhysicianAsync(int physicianId)
        {
            return await _appointmentRepository.GetAppointmentsByPhysicianAsync(physicianId);
        }

        public async Task<bool> IsTimeSlotAvailableAsync(DateTime dateTime, int physicianId)
        {
            IEnumerable<Appointment> existingAppointments = await _appointmentRepository
                .FindAsync(a => a.PhysicianId == physicianId && a.DateTime == dateTime);
            
            return !existingAppointments.Any();
        }
    }