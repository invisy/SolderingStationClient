using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation.Config;

public class ThermalProfilePartConfiguration : IEntityTypeConfiguration<ThermalProfilePartEntity>
{
    public void Configure(EntityTypeBuilder<ThermalProfilePartEntity> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}