using Chipsoft.Assignments.EPDConsole.Interfaces;
using Chipsoft.Assignments.EPDConsole.Models;

namespace Chipsoft.Assignments.EPDConsole.Services;

public class PhysicianService : IPhysicianService
{
    private readonly IPhysicianRepository _physicianRepository;

    public PhysicianService(IPhysicianRepository physicianRepository)
    {
        _physicianRepository = physicianRepository;
    }

    public async Task<IEnumerable<Physician>> GetAllPhysiciansAsync()
    {
        return await _physicianRepository.GetAllAsync();
    }

    public async Task<Physician> GetPhysicianByIdAsync(int id)
    {
        return await _physicianRepository.GetByIdAsync(id);
    }

    public async Task<Physician> AddPhysicianAsync(Physician physician)
    {
        if (string.IsNullOrWhiteSpace(physician.FirstName)) throw new ArgumentException("Voornaam is verplicht");
            
        if (string.IsNullOrWhiteSpace(physician.LastName)) throw new ArgumentException("Achternaam is verplicht");
        

        return await _physicianRepository.AddAsync(physician);
    }

    public async Task DeletePhysicianAsync(int id)
    {
        Physician? physician = await _physicianRepository.GetByIdAsync(id);
        
        if (physician == null) throw new ArgumentException("Arts niet gevonden");
                
        await _physicianRepository.DeleteAsync(id);
    }

    public async Task<bool> PhysicianExistsAsync(int id)
    {
        Physician? physician = await _physicianRepository.GetByIdAsync(id);
        
        return physician != null;
    }
}