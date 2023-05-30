using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.Models;

public class Device
{
    public Device(ulong id, string name, IEnumerable<TemperatureControllerKey> keys, bool supportsPid)
    {
        Id = id;
        Name = name;
        TemperatureControllersKeys = keys;
        SupportsPid = supportsPid;
    }

    public ulong Id { get; }
    public string Name { get; }
    public bool SupportsPid { get; }
    public IEnumerable<TemperatureControllerKey> TemperatureControllersKeys { get; }
}