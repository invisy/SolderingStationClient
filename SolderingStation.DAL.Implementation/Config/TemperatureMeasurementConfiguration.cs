using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation.Config;

public class TemperatureMeasurementPointConfiguration: IEntityTypeConfiguration<TemperatureMeasurementPointEntity>
{
    public void Configure(EntityTypeBuilder<TemperatureMeasurementPointEntity> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Temperature)
            .IsRequired();
        
        builder.Property(entity => entity.SecondsElapsed)
            .IsRequired();
    }
}