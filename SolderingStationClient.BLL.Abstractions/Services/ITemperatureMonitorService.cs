using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface ITemperatureMonitorService
{
    public event EventHandler<TemperatureMeasurementEventArgs>? NewTemperatureMeasurement;

    public void Enable();
    public void ChangeInterval(double ms);
    public void StartControllerTracking(TemperatureControllerKey temperatureControllerKey);
    public void StopControllerTracking(TemperatureControllerKey temperatureControllerKey);
}