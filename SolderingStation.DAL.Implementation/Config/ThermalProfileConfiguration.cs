using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation.Config;

public class ThermalProfileConfiguration : IEntityTypeConfiguration<ThermalProfileEntity>
{
    public void Configure(EntityTypeBuilder<ThermalProfileEntity> builder)
    {
        builder.HasKey(entity => entity.Id);
        
        builder.Property(entity => entity.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasMany(e => e.Parts)
            .WithOne().OnDelete(DeleteBehavior.ClientCascade);
    }
}