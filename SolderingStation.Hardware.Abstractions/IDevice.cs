namespace SolderingStation.Hardware.Abstractions;

public interface IDevice : IDisposable
{
    string Name { get; }
    bool Connect();
    Task<bool> Probe();
    void CloseConnection();
}