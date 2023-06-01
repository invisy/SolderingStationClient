using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation.Config;

public class SerialConnectionParametersConfiguration : IEntityTypeConfiguration<SerialConnectionParametersEntity>
{
    public void Configure(EntityTypeBuilder<SerialConnectionParametersEntity> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.PortName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(entity => entity.Parity)
            .IsRequired();
        
        builder.Property(entity => entity.BaudRate)
            .IsRequired();
        
        builder.Property(entity => entity.DataBits)
            .IsRequired();
        
        builder.Property(entity => entity.StopBits)
            .IsRequired();
    }
}