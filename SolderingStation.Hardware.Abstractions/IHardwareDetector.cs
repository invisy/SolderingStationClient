using SolderingStation.Hardware.Models.ConnectionParameters;

namespace SolderingStation.Hardware.Abstractions;

public interface IHardwareDetector<T> where T : IConnectionParameters
{
    Task<ulong> ConnectDeviceWithIdentification(T parameters);
}