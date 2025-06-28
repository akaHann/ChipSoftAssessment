using Chipsoft.Assignments.EPDConsole.Interfaces;
using Chipsoft.Assignments.EPDConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Repositories;

public class PatientRepository : Repository<Patient>, IPatientRepository
{
    public PatientRepository(EPDDbContext context) : base(context) { }

    public async Task<IEnumerable<Patient>> GetPatientsWithAppointmentsAsync()
    {
        return await _dbSet.Include(p => p.Appointments).ToListAsync();
    }
}