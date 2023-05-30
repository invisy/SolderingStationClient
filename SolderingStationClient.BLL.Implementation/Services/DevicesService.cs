using SolderingStation.Hardware.Abstractions;
using SolderingStation.Hardware.Abstractions.Capabilities;
using SolderingStation.Hardware.Models;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Implementation.Services;

public class DevicesService : IDevicesService
{
    private bool _isDisposed;
    private readonly IDeviceManager _deviceManager;

    public DevicesService(IDeviceManager deviceManager)
    {
        _deviceManager = deviceManager;
        _deviceManager.DevicePresenceChanged += OnDevicePresenceChanged;
    }

    public event EventHandler<DeviceConnectedEventArgs>? DeviceConnected;
    public event EventHandler<DeviceDisconnectedEventArgs>? DeviceDisconnected;

    public async Task<Device> GetDevice(ulong deviceId)
    {
        var controllersKeys = new List<TemperatureControllerKey>();

        var deviceName = GetDeviceName(deviceId);
        var connectionName = GetConnectionName(deviceId);
        var supportsPid = SupportsPid(deviceId);

        await foreach (var key in GetControllersKeys(deviceId))
            controllersKeys.Add(key);

        return new Device(deviceId, deviceName, connectionName, controllersKeys, supportsPid);
    }

    public async Task<IEnumerable<Device>> GetDevices()
    {
        var deviceIdsList = _deviceManager.GetAllDeviceIds();
        var devices = new List<Device>();

        foreach (var deviceId in deviceIdsList)
            devices.Add(await GetDevice(deviceId));

        return devices;
    }

    private string GetDeviceName(ulong deviceId)
    {
        return _deviceManager.GetDeviceNameById(deviceId);
    }
    
    private string GetConnectionName(ulong deviceId)
    {
        return _deviceManager.GetConnectionNameByDeviceId(deviceId);
    }

    private bool SupportsPid(ulong deviceId)
    {
        return _deviceManager.IsDeviceCapabilitySupported<IPidControllerCapability>(deviceId);
    }

    private async IAsyncEnumerable<TemperatureControllerKey> GetControllersKeys(ulong deviceId)
    {
        var capability = _deviceManager.TryGetDeviceCapability<ITemperatureControllerCapability>(deviceId);

        if (capability == null)
            yield break;

        var controllersNumber = await capability.GetChannelsNumber();
        for (byte i = 0; i < controllersNumber; i++)
            yield return new TemperatureControllerKey(deviceId, i);
    }

    private async void OnDevicePresenceChanged(object? sender, DevicePresenceChangedEventArgs<ulong> args)
    {
        if (args.Status == PresenceStatus.Connected)
        {
            try
            {
                var device = await GetDevice(args.DeviceId);
                DeviceConnected?.Invoke(this, new DeviceConnectedEventArgs(device));
            }
            catch (Exception e)
            {
                _deviceManager.RemoveDevice(args.DeviceId);
            }
        }
        else
            DeviceDisconnected?.Invoke(this, new DeviceDisconnectedEventArgs(args.DeviceId));
    }

    public void Dispose()
    {
        Dispose(true);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing)
            _deviceManager.DevicePresenceChanged -= OnDevicePresenceChanged;
            
        _isDisposed = true;
    }
}