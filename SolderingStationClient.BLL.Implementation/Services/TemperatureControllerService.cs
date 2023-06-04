using SolderingStation.Hardware.Abstractions;
using SolderingStation.Hardware.Abstractions.Capabilities;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.BLL.Implementation.Exceptions;
using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Implementation.Services;

public class TemperatureControllerService : ITemperatureControllerService
{
    private readonly IDeviceManager _deviceManager;

    public TemperatureControllerService(IDeviceManager deviceManager)
    {
        _deviceManager = deviceManager;
    }

    public async Task<TemperatureController> GetTemperatureController(TemperatureControllerKey controllerKey)
    {
        var currentTemperature = await GetCurrentTemperature(controllerKey);
        var desiredTemperature = await GetDesiredTemperature(controllerKey);

        return new TemperatureController(controllerKey, currentTemperature, desiredTemperature);
    }

    public async Task SetDesiredTemperature(TemperatureControllerKey controllerKey, ushort value)
    {
        var handle = GetHandle(controllerKey.DeviceId);
        await handle.SetDesiredTemperature(controllerKey.ChannelId, value);
    }

    private async Task<ushort> GetCurrentTemperature(TemperatureControllerKey controllerKey)
    {
        var handle = GetHandle(controllerKey.DeviceId);
        return await handle.GetCurrentTemperature(controllerKey.ChannelId);
    }

    private async Task<ushort> GetDesiredTemperature(TemperatureControllerKey controllerKey)
    {
        var handle = GetHandle(controllerKey.DeviceId);
        return await handle.GetDesiredTemperature(controllerKey.ChannelId);
    }

    private ITemperatureControllerCapability GetHandle(ulong deviceId)
    {
        var handle = _deviceManager.TryGetDeviceCapability<ITemperatureControllerCapability>(deviceId);

        if (handle == null)
            throw new CapabilityIsNotSupportedException();

        return handle;
    }
}