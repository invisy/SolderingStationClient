using System.Drawing;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Implementation.Services;

public class ThermalProfileService : IThermalProfileService
{
    private List<ThermalProfile> testProfiles = new()
    {
        new ThermalProfile(1, "TestProfile", new []
        {
           new ControllerThermalProfile(1, "BottomHeater", Color.Green.ToArgb(), new []
           {
               new TemperatureMeasurement(0, 0),
               new TemperatureMeasurement(5, 10),
               new TemperatureMeasurement(10, 40)
           }) 
        }),
        new ThermalProfile(2, "TestProfile2", new []
        {
            new ControllerThermalProfile(1, "BottomHeater", Color.Green.ToArgb(), new []
            {
                new TemperatureMeasurement(0, 0),
                new TemperatureMeasurement(17, 5),
                new TemperatureMeasurement(25, 40)
            }),
            new ControllerThermalProfile(1, "TopHeater", Color.Blue.ToArgb(), new []
            {
                new TemperatureMeasurement(0, 0),
                new TemperatureMeasurement(9, 40),
            }) 
        })
    };

    public IEnumerable<ThermalProfile> GetAll()
    {
        return testProfiles;
    }

    public ThermalProfile? GetById(int thermalProfileId)
    {
        return testProfiles.FirstOrDefault(profile => profile.Id == thermalProfileId);
    }

    public void Add(ThermalProfile thermalProfile)
    {
        testProfiles.Add(thermalProfile);
    }

    public void Remove(int thermalProfileId)
    {
        ThermalProfile? found = testProfiles.FirstOrDefault(profile => profile.Id == thermalProfileId);
        if (found != null)
            testProfiles.Remove(found);
    }

    public void Update(ThermalProfile thermalProfile)
    {
        ThermalProfile? found = testProfiles.FirstOrDefault(profile => profile.Id == thermalProfile.Id);
        if (found != null)
            testProfiles.Remove(found);
        testProfiles.Add(thermalProfile);
    }
}