using Chipsoft.Assignments.EPDConsole.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chipsoft.Assignments.EPDConsole.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.Id).IsUnique();
        builder.Property(a => a.DateTime).IsRequired();
        builder.Property(a => a.Notes).HasMaxLength(500);
                
        // Foreign key relationships
        builder.HasOne(e => e.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(e => e.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
                    
        builder.HasOne(e => e.Physician)
            .WithMany(p => p.Appointments)
            .HasForeignKey(e => e.PhysicianId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}