namespace SolderingStationClient.Models.TemperatureControllers;

public class TemperatureControllerInfo
{
    public TemperatureControllerInfo(TemperatureControllerKey controllerId, string name)
    {
        ControllerId = controllerId;
        DeviceName = name;
    }

    public TemperatureControllerKey ControllerId { get; }
    public string DeviceName { get; }
}