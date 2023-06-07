using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.Models;

public class ThermalProfileControllerBinding
{
    public ControllerThermalProfile ControllerThermalProfile { get; }
    public TemperatureControllerKey TemperatureControllerKey { get; }

    public ThermalProfileControllerBinding(ControllerThermalProfile controllerThermalProfile, TemperatureControllerKey temperatureControllerKey)
    {
        ControllerThermalProfile = controllerThermalProfile;
        TemperatureControllerKey = temperatureControllerKey;
    }
    
}