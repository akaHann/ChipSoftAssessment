using Chipsoft.Assignments.EPDConsole.Models;

namespace Chipsoft.Assignments.EPDConsole.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<Patient>> GetAllPatientsAsync();
    Task<Patient> GetPatientByIdAsync(int id);
    Task<Patient> AddPatientAsync(Patient patient);
    Task DeletePatientAsync(int id);
    Task<bool> PatientExistsAsync(int id);
}