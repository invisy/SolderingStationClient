using SolderingStation.Hardware.Models.ConnectionParameters;

namespace SolderingStation.Hardware.Abstractions;

public interface IDeviceFactory<T> where T : IConnectionParameters
{
    IDevice Create(T parameters);
}