using System.Collections.Concurrent;
using SolderingStation.Hardware.Abstractions;
using SolderingStation.Hardware.Abstractions.Connections;
using SolderingStation.Hardware.Models;

namespace SolderingStation.Hardware.Implementation;

public class DeviceManager : IDeviceManager
{
    private readonly ConcurrentDictionary<ulong, IDevice> _connectedDevices = new();
    private ulong _lastId;

    public string GetDeviceNameById(ulong id)
    {
        return _connectedDevices[id].Name;
    }

    public IEnumerable<ulong> GetAllDeviceIds()
    {
        return _connectedDevices.Keys;
    }

    public IEnumerable<CapabilityHandle<T>> GetByDeviceCapability<T>() where T : class, IDeviceCapability
    {
        foreach (var deviceKey in _connectedDevices.Keys)
            if (_connectedDevices[deviceKey] is T)
                yield return new CapabilityHandle<T>(deviceKey, (T)_connectedDevices[deviceKey]);
    }

    public IEnumerable<CapabilityHandle<T>> GetByConnectionCapability<T>() where T : class, IConnectionCapability
    {
        throw new NotImplementedException();
    }

    public bool IsConnectionCapabilitySupported<T>(ulong id) where T : IDeviceCapability
    {
        throw new NotImplementedException();
    }

    public T? TryGetConnectionCapability<T>(ulong id) where T : IConnectionCapability
    {
        throw new NotImplementedException();
    }

    public bool IsDeviceCapabilitySupported<T>(ulong id) where T : IDeviceCapability
    {
        _connectedDevices.TryGetValue(id, out var device);
        return device is T;
    }

    public T? TryGetDeviceCapability<T>(ulong id) where T : IDeviceCapability
    {
        var capabilityInterface = default(T);

        _connectedDevices.TryGetValue(id, out var device);

        if (device is T value)
            capabilityInterface = value;

        return capabilityInterface;
    }

    public ulong AddDevice(IDevice device)
    {
        var id = Interlocked.Add(ref _lastId, 1);

        if (!_connectedDevices.TryAdd(id, device))
            throw new ArgumentException("Unable to add connection.", nameof(device));

        DevicePresenceChanged?.Invoke(this, new DevicePresenceChangedEventArgs<ulong>(id, PresenceStatus.Connected));
        return id;
    }

    public void RemoveDevice(ulong id)
    {
        if (!_connectedDevices.TryRemove(id, out var device))
            throw new ArgumentException("Unable to remove connection.", nameof(id));

        device.CloseConnection();
        DevicePresenceChanged?.Invoke(this, new DevicePresenceChangedEventArgs<ulong>(id, PresenceStatus.Disconnected));
    }

    public event EventHandler<DevicePresenceChangedEventArgs<ulong>>? DevicePresenceChanged;
}