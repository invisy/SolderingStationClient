namespace SolderingStation.Hardware.Abstractions.Connections;

public interface IConnection : IDisposable
{
    string Name { get; }
}