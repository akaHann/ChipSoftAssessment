using Chipsoft.Assignments.EPDConsole.Interfaces;
using Chipsoft.Assignments.EPDConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole.Repositories;

public class PhysicianRepository : Repository<Physician>, IPhysicianRepository
{
    public PhysicianRepository(EPDDbContext context) : base(context) { }

    public async Task<IEnumerable<Physician>> GetPhysiciansWithAppointmentsAsync()
    {
        return await _dbSet.Include(p => p.Appointments).ToListAsync();
    }
}