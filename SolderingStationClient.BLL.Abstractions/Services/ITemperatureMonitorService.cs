using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface ITemperatureMonitorService
{
    public event EventHandler<TemperatureMeasurementEventArgs>? NewTemperatureMeasurement;

    public void Enable();
    public void StartControllerTracking(TemperatureControllerKey temperatureControllerKey);
    public void StopControllerTracking(TemperatureControllerKey temperatureControllerKey);
    public void StopDeviceTracking(ulong deviceId);
}