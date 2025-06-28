
namespace Chipsoft.Assignments.EPDConsole.Models;

public class Patient
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Address { get; set; }
    
    public string PostalCode { get; set; }
    
    public string City { get; set; }
        
    // Invoicing
    public string InsuranceNumber { get; set; }
        
    public string FullName => $"{FirstName} {LastName}";
        
    // Navigation property
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}