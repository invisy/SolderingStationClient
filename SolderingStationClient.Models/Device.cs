using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.Models;

public class Device
{
    public Device(ulong id, string name, string connectionName, IEnumerable<TemperatureControllerKey> keys, bool supportsPid)
    {
        Id = id;
        Name = name;
        TemperatureControllersKeys = keys;
        SupportsPid = supportsPid;
        ConnectionName = connectionName;
    }

    public ulong Id { get; }
    public string Name { get; }
    public string ConnectionName { get; }
    public bool SupportsPid { get; }
    public IEnumerable<TemperatureControllerKey> TemperatureControllersKeys { get; }
}