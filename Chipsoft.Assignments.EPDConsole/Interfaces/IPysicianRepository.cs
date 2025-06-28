using Chipsoft.Assignments.EPDConsole.Models;

namespace Chipsoft.Assignments.EPDConsole.Interfaces;

public interface IPhysicianRepository : IRepository<Physician>
{
    Task<IEnumerable<Physician>> GetPhysiciansWithAppointmentsAsync();
}