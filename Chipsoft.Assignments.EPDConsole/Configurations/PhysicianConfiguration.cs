using Chipsoft.Assignments.EPDConsole.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chipsoft.Assignments.EPDConsole.Configurations;

public class PhysicianConfiguration : IEntityTypeConfiguration<Physician>
{
    public void Configure(EntityTypeBuilder<Physician> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Id).IsUnique();
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(30);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(30);
        builder.Property(e => e.PhoneNumber).HasMaxLength(20);
    }
}