
namespace Chipsoft.Assignments.EPDConsole.Models;

public class Appointment
{
    public int Id { get; set; }
        
    public DateTime DateTime { get; set; }
        
    public int PatientId { get; set; }
        
    public int PhysicianId { get; set; }
        
    public string Notes { get; set; }
        
    // Navigation properties
    public virtual Patient Patient { get; set; }
    public virtual Physician Physician { get; set; }
}