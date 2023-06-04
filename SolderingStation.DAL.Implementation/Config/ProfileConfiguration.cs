using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation.Config;

public class ProfileConfiguration : IEntityTypeConfiguration<ProfileEntity>
{
    public void Configure(EntityTypeBuilder<ProfileEntity> builder)
    {
        builder.HasKey(entity => entity.Id);
        
        builder.Property(entity => entity.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(entity => entity.Language).WithMany().HasForeignKey(entity => entity.LanguageId)
            .IsRequired();
        
        builder.HasMany<ThermalProfileEntity>().WithOne().HasForeignKey(entity => entity.ProfileId)
            .IsRequired();
        
        builder.HasMany<SerialConnectionParametersEntity>().WithOne().HasForeignKey(entity => entity.ProfileId)
            .IsRequired();
    }
}