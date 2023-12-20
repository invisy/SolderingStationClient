using System.Drawing;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation;

public static class SolderingStationContextSeed
{
    public static void Seed(this SolderingStationDbContext dbContext)
    {
        if (!dbContext.CollectionExists(nameof(LanguageEntity)))
        {
            var languages = new List<LanguageEntity>()
            {
                new LanguageEntity(1, "English", "English", "en"),
                new LanguageEntity(2, "Ukrainian", "Українська", "uk")
            };
            
            dbContext.Languages.InsertBulk(languages);
            dbContext.Commit();
        }
        
        if (!dbContext.CollectionExists(nameof(ProfileEntity)))
        {

            var thermalProfile = new ThermalProfileEntity(1, "TestProfile", new[]
            {
                new ThermalProfilePartEntity(1, "BottomHeater", Color.Green.ToArgb(), new[]
                {
                    new TemperatureMeasurementPointEntity(1, 0, 0),
                    new TemperatureMeasurementPointEntity(2, 10, 30),
                    new TemperatureMeasurementPointEntity(3, 20, 35),
                    new TemperatureMeasurementPointEntity(4, 30, 35),
                    new TemperatureMeasurementPointEntity(5, 50, 40)
                }),
                new ThermalProfilePartEntity(2, "TopHeater", Color.Blue.ToArgb(), new[]
                {
                    new TemperatureMeasurementPointEntity(1, 0, 0),
                    new TemperatureMeasurementPointEntity(2, 9, 40),
                })
            }, 1);

            var thermalProfiles = new List<ThermalProfileEntity>();
            thermalProfiles.Add(thermalProfile);
            
            var profile = new ProfileEntity(1, "Main", 1, thermalProfiles);
            dbContext.Profiles.Insert(profile);
            dbContext.Commit();
        }
        
    }
}