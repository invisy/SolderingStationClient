using Microsoft.EntityFrameworkCore;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation;

public static class SolderingStationContextSeed
{
    public static void Seed(this SolderingStationDbContext dbContext)
    {
        if (dbContext.Database.IsSqlite())
        {
            dbContext.Database.Migrate();
        }
        
        if (!dbContext.Languages.Any())
        {
            dbContext.Languages.AddRange(
                new LanguageEntity("English", "English", "en"),
                new LanguageEntity("Ukrainian", "Українська", "uk")
                );
            dbContext.SaveChanges();
        }
        
        if (!dbContext.Profiles.Any())
        {
            dbContext.Profiles.Add(new ProfileEntity("Main", 1));
            dbContext.SaveChanges();
        }

    }
}