
namespace Chipsoft.Assignments.EPDConsole.Models;

public class Physician
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string PhoneNumber { get; set; }
        
    public string FullName => $"Dr. {FirstName} {LastName}";
        
    // Navigation property
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}