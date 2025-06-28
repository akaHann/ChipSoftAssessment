using Chipsoft.Assignments.EPDConsole.Interfaces;
using Chipsoft.Assignments.EPDConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Repositories;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(EPDDbContext context) : base(context) { }

    public async Task<IEnumerable<Appointment>> GetAppointmentsWithDetailsAsync()
    {
        return await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Physician)
            .OrderBy(a => a.DateTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId)
    {
        return await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Physician)
            .Where(a => a.PatientId == patientId)
            .OrderBy(a => a.DateTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByPhysicianAsync(int physicianId)
    {
        return await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Physician)
            .Where(a => a.PhysicianId == physicianId)
            .OrderBy(a => a.DateTime)
            .ToListAsync();
    }
}