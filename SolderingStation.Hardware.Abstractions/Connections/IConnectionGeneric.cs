using SolderingStation.Hardware.Models.ConnectionParameters;

namespace SolderingStation.Hardware.Abstractions.Connections;

public interface IConnectionGeneric<TConnectionParameters> : IConnection
    where TConnectionParameters : IConnectionParameters
{
    TConnectionParameters ConnectionParameters { get; }
    bool Connect();
    void Disconnect();
}