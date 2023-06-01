using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation.Config;

public class LanguageConfiguration : IEntityTypeConfiguration<LanguageEntity>
{
    public void Configure(EntityTypeBuilder<LanguageEntity> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.EnglishName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(entity => entity.NativeName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(entity => entity.Code)
            .IsRequired()
            .HasMaxLength(5);
    }
}