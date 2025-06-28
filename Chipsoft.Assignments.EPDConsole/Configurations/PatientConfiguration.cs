using Chipsoft.Assignments.EPDConsole.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chipsoft.Assignments.EPDConsole.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.Id).IsUnique();
        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(30);
        builder.Property(p => p.LastName).IsRequired().HasMaxLength(30);
        builder.Property(e => e.DateOfBirth).IsRequired();
        builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Address).IsRequired().HasMaxLength(50);
        builder.Property(p =>p.PostalCode).IsRequired().HasMaxLength(10);
        builder.Property(p => p.City).IsRequired().HasMaxLength(25);
        builder.Property(p => p.InsuranceNumber).HasMaxLength(50);
    }
}