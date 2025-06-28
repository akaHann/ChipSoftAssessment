using Chipsoft.Assignments.EPDConsole.Models;

namespace Chipsoft.Assignments.EPDConsole.Interfaces;

public interface IPhysicianService
{
    Task<IEnumerable<Physician>> GetAllPhysiciansAsync();
    Task<Physician> GetPhysicianByIdAsync(int id);
    Task<Physician> AddPhysicianAsync(Physician physician);
    Task DeletePhysicianAsync(int id);
    Task<bool> PhysicianExistsAsync(int id);
}