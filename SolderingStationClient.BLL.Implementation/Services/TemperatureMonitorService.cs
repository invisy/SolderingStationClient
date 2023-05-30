using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Implementation.Services;

public class TemperatureMonitorService : ITemperatureMonitorService
{
    private readonly ITemperatureControllerService _temperatureControllerService;
    private readonly ITimer _timer;
    private readonly ITemperatureHistoryTracker _temperatureHistoryTracker;
    private readonly List<TemperatureControllerKey> _trackingControllers = new List<TemperatureControllerKey>();
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1,1);

    private DateTime StartTime;
    
    public TemperatureMonitorService(
        ITemperatureControllerService temperatureControllerService,
        ITemperatureHistoryTracker temperatureHistoryTracker,
        ITimer timer)
    {
        _temperatureHistoryTracker = temperatureHistoryTracker;
        _temperatureControllerService = temperatureControllerService;
        _timer = timer;
    }

    public event EventHandler<TemperatureMeasurementEventArgs>? NewTemperatureMeasurement;
    
    public void Enable()
    {
        StartTime = DateTime.Now;
        _timer.Interval = 1000;
        _timer.Start();
        _timer.TimerIntervalElapsed += Tick;
    }

    public void StartControllerTracking(TemperatureControllerKey temperatureControllerKey)
    {
        _semaphoreSlim.Wait();
        _trackingControllers.Add(temperatureControllerKey);
        _semaphoreSlim.Release();
    }

    public void StopControllerTracking(TemperatureControllerKey temperatureControllerKey)
    {
        _semaphoreSlim.Wait();
        _trackingControllers.Remove(temperatureControllerKey);
        _temperatureHistoryTracker.RemoveControllerHistory(temperatureControllerKey);
        _semaphoreSlim.Release();
    }

    public void StopDeviceTracking(ulong deviceId)
    {
        _semaphoreSlim.Wait();
        var controllersKeys = _trackingControllers.Where(controller => 
            controller.DeviceId == deviceId).ToList();
        
        foreach (var key in controllersKeys)
        {
            _trackingControllers.Remove(key);
            _temperatureHistoryTracker.RemoveControllerHistory(key);
        }
        
        _semaphoreSlim.Release();
    }
    
    private async void Tick(object? sender, DateTime time)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            var tasks = _trackingControllers.Select(async key =>
            {
                await MeasureTemperature(key, time);
            });
            await Task.WhenAll(tasks);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private async Task MeasureTemperature(TemperatureControllerKey key, DateTime time)
    {
        try
        {
            var temperatureController = await _temperatureControllerService.GetTemperatureController(key);
            var elapsed = ((float)(time - StartTime).Milliseconds) / 1000;
            var temperatureMeasurement = new TemperatureMeasurement(elapsed, temperatureController.CurrentTemperature);
            var desiredTemperature = temperatureController.DesiredTemperature;
            _temperatureHistoryTracker.AddMeasurement(key, temperatureMeasurement);
            NewTemperatureMeasurement?.Invoke(this,
                new TemperatureMeasurementEventArgs(key, temperatureMeasurement, desiredTemperature));
        }
        catch (Exception)
        {
            // ignored
        }
    }
}