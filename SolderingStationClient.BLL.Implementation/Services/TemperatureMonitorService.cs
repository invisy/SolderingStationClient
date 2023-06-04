using SolderingStation.Hardware.Abstractions;
using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Implementation.Services;

public class TemperatureMonitorService : ITemperatureMonitorService
{
    private readonly IDevicesService _devicesService;
    private readonly ITemperatureControllerService _temperatureControllerService;
    private readonly ITimer _timer;
    private readonly ITemperatureHistoryTracker _temperatureHistoryTracker;
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1,1);
    
    private readonly Dictionary<TemperatureControllerKey, bool> _trackedControllers = new();

    private DateTime StartTime;
    
    public TemperatureMonitorService(
        IDevicesService devicesService,
        ITemperatureControllerService temperatureControllerService,
        ITemperatureHistoryTracker temperatureHistoryTracker,
        ITimer timer)
    {
        _devicesService = devicesService;
        _temperatureHistoryTracker = temperatureHistoryTracker;
        _temperatureControllerService = temperatureControllerService;
        _timer = timer;
        _devicesService.DeviceConnected += OnDeviceConnected;
        _devicesService.DeviceDisconnected -= OnDeviceDisconnected;
    }

    public event EventHandler<TemperatureMeasurementEventArgs>? NewTemperatureMeasurement;
    
    public void Enable()
    {
        StartTime = DateTime.Now;
        _timer.Interval = 1000;
        _timer.Start();
        _timer.TimerIntervalElapsed += Tick;
    }

    public void ChangeInterval(double ms)
    {
        _timer.Stop();
        _timer.Interval = ms;
        _timer.Start();
    }

    public void StartControllerTracking(TemperatureControllerKey temperatureControllerKey)
    {
        SwitchControllerState(temperatureControllerKey, true);
    }

    public void StopControllerTracking(TemperatureControllerKey temperatureControllerKey)
    {
        SwitchControllerState(temperatureControllerKey, false);
    }

    private void OnDeviceConnected(object? sender, DeviceConnectedEventArgs args)
    {
        _semaphoreSlim.Wait();
        var controllerKeys = args.Device.TemperatureControllersKeys;
        
        foreach (var key in controllerKeys)
            _trackedControllers[key] = true;

        _semaphoreSlim.Release();
    }
    
    private void OnDeviceDisconnected(object? sender, DeviceDisconnectedEventArgs args)
    {
        _semaphoreSlim.Wait();
        var controllersKeys = _trackedControllers.Keys.Where(controller => 
            controller.DeviceId == args.DeviceId).ToList();
        
        foreach (var key in controllersKeys)
        {
            _trackedControllers.Remove(key);
            _temperatureHistoryTracker.RemoveControllerHistory(key);
        }
        
        _semaphoreSlim.Release();
    }

    private void SwitchControllerState(TemperatureControllerKey temperatureControllerKey, bool state)
    {
        var containsKey = true;
        
        _semaphoreSlim.Wait();
        if (_trackedControllers.ContainsKey(temperatureControllerKey))
            _trackedControllers[temperatureControllerKey] = state;
        else
            containsKey = false;
        _semaphoreSlim.Release();

        if (!containsKey)
            throw new ArgumentException($"{nameof(temperatureControllerKey)} is not valid");
    }
    
    private async void Tick(object? sender, DateTime time)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            var tasks = _trackedControllers.Select(async key =>
            {
                if(key.Value)
                    await MeasureTemperature(key.Key, time);
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