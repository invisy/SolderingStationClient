using SolderingStation.Hardware.Abstractions;
using SolderingStation.Hardware.Abstractions.Capabilities;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Implementation.Services;

public class PidTemperatureControllerService : TemperatureControllerService, IPidTemperatureControllerService
{
    private readonly IDeviceManager _deviceManager;

    public PidTemperatureControllerService(IDeviceManager deviceManager) : base(deviceManager)
    {
        _deviceManager = deviceManager;
    }

    public async Task<PidTemperatureController> GetPidTemperatureController(TemperatureControllerKey controllerKey)
    {
        var temperatureController = await GetTemperatureController(controllerKey);
        var coefficients = await GetCoefficients(controllerKey);

        return new PidTemperatureController(temperatureController, coefficients);
    }

    public async Task<PidControllerCoefficients> GetCoefficients(TemperatureControllerKey controllerKey)
    {
        var handle = GetHandle(controllerKey.DeviceId);
        var pidCoefficients = await handle.GetCoefficients(controllerKey.ChannelId);

        return new PidControllerCoefficients(pidCoefficients.Kp, pidCoefficients.Ki, pidCoefficients.Kd);
    }

    public async Task SetPidKp(TemperatureControllerKey controllerKey, float value)
    {
        var handle = GetHandle(controllerKey.DeviceId);
        await handle.SetKpCoefficient(controllerKey.ChannelId, value);
    }

    public async Task SetPidKi(TemperatureControllerKey controllerKey, float value)
    {
        var handle = GetHandle(controllerKey.DeviceId);
        await handle.SetKiCoefficient(controllerKey.ChannelId, value);
    }

    public async Task SetPidKd(TemperatureControllerKey controllerKey, float value)
    {
        var handle = GetHandle(controllerKey.DeviceId);
        await handle.SetKdCoefficient(controllerKey.ChannelId, value);
    }

    private IPidControllerCapability GetHandle(ulong deviceId)
    {
        var handle = _deviceManager.TryGetDeviceCapability<IPidControllerCapability>(deviceId);

        if (handle == null)
            throw new NullReferenceException(nameof(deviceId));

        return handle;
    }
}