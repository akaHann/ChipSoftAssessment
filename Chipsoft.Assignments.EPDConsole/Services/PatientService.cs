using Chipsoft.Assignments.EPDConsole.Interfaces;
using Chipsoft.Assignments.EPDConsole.Models;

namespace Chipsoft.Assignments.EPDConsole.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
    {
        return await _patientRepository.GetAllAsync();
    }

    public async Task<Patient> GetPatientByIdAsync(int id)
    {
        return await _patientRepository.GetByIdAsync(id);
    }

    public async Task<Patient> AddPatientAsync(Patient patient)
    {
        if (string.IsNullOrWhiteSpace(patient.FirstName)) throw new ArgumentException("Voornaam is verplicht");
            
        if (string.IsNullOrWhiteSpace(patient.LastName)) throw new ArgumentException("Achternaam is verplicht");
                
        if (patient.DateOfBirth > DateTime.Now) throw new ArgumentException("Geboortedatum kan niet in de toekomst zijn");

        return await _patientRepository.AddAsync(patient);
    }

    public async Task DeletePatientAsync(int id)
    {
        Patient? patient = await _patientRepository.GetByIdAsync(id);
        
        if (patient == null) throw new ArgumentException("Patiënt niet gevonden");
                
        await _patientRepository.DeleteAsync(id);
    }

    public async Task<bool> PatientExistsAsync(int id)
    {
        Patient? patient = await _patientRepository.GetByIdAsync(id);
        
        return patient != null;
    }
}