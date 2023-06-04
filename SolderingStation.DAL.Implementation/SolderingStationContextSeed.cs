using System.Drawing;
using Microsoft.EntityFrameworkCore;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation;

public static class SolderingStationContextSeed
{
    public static void Seed(this SolderingStationDbContext dbContext)
    {
        if (dbContext.Database.IsSqlite())
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            //dbContext.Database.Migrate();
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
    
        if (!dbContext.ThermalProfiles.Any())
        {
            var thermalProfile = new ThermalProfileEntity(1, "TestProfile", new[]
            {
                new ThermalProfilePartEntity(0, "BottomHeater", Color.Green.ToArgb(), new[]
                {
                    new TemperatureMeasurementPointEntity(0, 0, 0),
                    new TemperatureMeasurementPointEntity(0, 17, 5),
                    new TemperatureMeasurementPointEntity(0, 25, 40)
                }),
                new ThermalProfilePartEntity(0, "TopHeater", Color.Blue.ToArgb(), new[]
                {
                    new TemperatureMeasurementPointEntity(0, 0, 0),
                    new TemperatureMeasurementPointEntity(0, 9, 40),
                })
            });
            thermalProfile.ProfileId = 1;

            dbContext.ThermalProfiles.Add(thermalProfile);

            dbContext.SaveChanges();
        }
    }
}