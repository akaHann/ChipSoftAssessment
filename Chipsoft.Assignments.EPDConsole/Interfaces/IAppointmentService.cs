using Chipsoft.Assignments.EPDConsole.Models;

namespace Chipsoft.Assignments.EPDConsole.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    Task<Appointment> AddAppointmentAsync(Appointment appointment);
    Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId);
    Task<IEnumerable<Appointment>> GetAppointmentsByPhysicianAsync(int physicianId);
    Task<bool> IsTimeSlotAvailableAsync(DateTime dateTime, int physicianId);
}