using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface ITemperatureMonitorService : IDisposable
{
    public bool IsRunning { get; }
    public event EventHandler<TemperatureMeasurementEventArgs>? NewTemperatureMeasurement;

    public void Enable(double interval);
    public void Disable();
    public void ClearHistory();
    public void ChangeInterval(double ms);
    public void StartControllerTracking(TemperatureControllerKey temperatureControllerKey);
    public void StopControllerTracking(TemperatureControllerKey temperatureControllerKey);
    void StartAllControllersTracking();
    public void StopAllControllersTracking();
}