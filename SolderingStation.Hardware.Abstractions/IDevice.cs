using SolderingStation.Hardware.Abstractions.Connections;

namespace SolderingStation.Hardware.Abstractions;

public interface IDevice
{
    IConnection Connection { get; }
    string Name { get; }
    bool Connect();
    Task<bool> Probe();
    void CloseConnection();
}