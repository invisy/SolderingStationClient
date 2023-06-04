using SolderingStation.Hardware.Abstractions.Connections;
using SolderingStation.Hardware.Models;

namespace SolderingStation.Hardware.Abstractions;

public interface IDeviceManager
{
    string GetDeviceNameById(ulong id);
    IEnumerable<ulong> GetAllDeviceIds();
    IEnumerable<CapabilityHandle<T>> GetByDeviceCapability<T>() where T : class, IDeviceCapability;
    bool IsDeviceCapabilitySupported<T>(ulong id) where T : IDeviceCapability;
    T? TryGetDeviceCapability<T>(ulong id) where T : IDeviceCapability;
    ulong AddDevice(IDevice device);
    void RemoveDevice(ulong id);

    string GetConnectionNameByDeviceId(ulong id);
    IEnumerable<CapabilityHandle<T>> GetByConnectionCapability<T>() where T : class, IConnectionCapability;
    bool IsConnectionCapabilitySupported<T>(ulong id) where T : IConnectionCapability;
    T? TryGetConnectionCapability<T>(ulong id) where T : IConnectionCapability;
    
    event EventHandler<DevicePresenceChangedEventArgs<ulong>> DevicePresenceChanged;
}