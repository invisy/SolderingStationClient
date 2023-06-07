using System.Collections.Concurrent;
using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.Jobs;
using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Implementation.Jobs;

public class ThermalProfileProcessingJob : IThermalProfileProcessingJob
{
    private const int UpdateTimeMs = 500;
    
    private readonly CancellationTokenSource _tokenSource = new();
    private readonly ITemperatureMonitorService _temperatureMonitor;
    private readonly ITemperatureControllerService _temperatureControllerService;
    private readonly IEnumerable<ThermalProfileControllerBinding> _thermalProfileControllerBindings;
    
    private int _errors;
    private float _currentProgress;
    private float _maxTimeInSeconds;
    private JobState _state = JobState.NotStarted;

    private ConcurrentDictionary<TemperatureControllerKey, bool> _controllersInWork = new();

    public JobState State
    {
        get => _state;
        private set
        {
            _state = value;
            StateChanged?.Invoke(this, new JobStateChangedEventArgs(_state));
        }
    }

    public ThermalProfileProcessingJob(
        ITemperatureMonitorService temperatureMonitor, 
        ITemperatureControllerService temperatureControllerService,
        IEnumerable<ThermalProfileControllerBinding> thermalProfileControllerBindings)
    {
        _temperatureMonitor = temperatureMonitor;
        _temperatureControllerService = temperatureControllerService;
        _thermalProfileControllerBindings = thermalProfileControllerBindings;

        _maxTimeInSeconds = FindLastProcessingPoint();
    }

    public float CurrentProgress
    {
        get => _currentProgress;
        private set
        {
            _currentProgress = value;
            ProgressUpdated?.Invoke(this, new JobProgressUpdatedEventArgs(_currentProgress));
        }
    }

    public event EventHandler<JobStateChangedEventArgs>? StateChanged;
    public event EventHandler<JobProgressUpdatedEventArgs>? ProgressUpdated;
    
    public async Task RunAsync()
    {
        State = JobState.Running;
        await Task.Run(Processing, _tokenSource.Token);
        
        if (_controllersInWork.Values.Contains(true))
            State = JobState.Finished;
        else
            State = _errors > 0 ? JobState.Failed : JobState.Stopped;
    }

    public void Cancel()
    {
        _tokenSource.Cancel();
        Thread.Sleep(500);
        DisableTemperatureMeasurements();
        State = JobState.Stopped;
    }

    private float FindLastProcessingPoint()
    {
        float maxPoint = 0;
        foreach (var thermalProfileControllerBinding in _thermalProfileControllerBindings)
        {
            var newPoint = thermalProfileControllerBinding.ControllerThermalProfile.TemperatureMeasurements.Last().SecondsElapsed;
            if (maxPoint < newPoint)
                maxPoint = newPoint;
        }

        return maxPoint;
    }

    private async Task Processing()
    {
        PrepareTemperatureMeasurements();
        while(_controllersInWork.Values.Contains(true) && _errors == 0 && !_tokenSource.Token.IsCancellationRequested)
            await Task.Delay(100);
        DisableTemperatureMeasurements();
    }

    private void PrepareTemperatureMeasurements()
    {
        _temperatureMonitor.Disable();
        _temperatureMonitor.StopAllControllersTracking();
        _temperatureMonitor.NewTemperatureMeasurement += OnTemperatureMeasurement;
        
        foreach (var thermalProfileController in _thermalProfileControllerBindings)
        {
            _controllersInWork.TryAdd(thermalProfileController.TemperatureControllerKey, true);
            _temperatureMonitor.StartControllerTracking(thermalProfileController.TemperatureControllerKey);
        }

        _temperatureMonitor.Enable(UpdateTimeMs);
    }
    
    private void DisableTemperatureMeasurements()
    {
        _temperatureMonitor.Disable();
        _temperatureMonitor.StartAllControllersTracking();
        _temperatureMonitor.NewTemperatureMeasurement -= OnTemperatureMeasurement;
    }

    private void OnTemperatureMeasurement(object? sender, TemperatureMeasurementEventArgs args)
    {
        if (_controllersInWork[args.TemperatureControllerId] == false)
            return;
        try
        {
            var binding = _thermalProfileControllerBindings.FirstOrDefault(binding =>
                binding.TemperatureControllerKey == args.TemperatureControllerId);

            if (binding == null)
            {
                Interlocked.Add(ref _errors, 1); //error
                return;
            }

            var temperature = NextTemperature(binding.ControllerThermalProfile.TemperatureMeasurements, args.Temperature.SecondsElapsed);
        
            // Done
            if (temperature.Item1 == false)
                while (true)
                    if (_controllersInWork.TryUpdate(args.TemperatureControllerId, false, true)) break;

            _temperatureControllerService.SetDesiredTemperature(args.TemperatureControllerId, temperature.Item2);
            CurrentProgress = args.Temperature.SecondsElapsed / _maxTimeInSeconds;
        }
        catch (Exception)
        {
            Interlocked.Add(ref _errors, 1); //error
        }
    }

    private (bool, ushort) NextTemperature(IEnumerable<ThermalProfilePoint> thermalProfileCurve, float secondsElapsed)
    {
        var thermalProfileCurveList = thermalProfileCurve.ToList();
        const float interpolationTimeSeconds = (float)UpdateTimeMs/1000*2;

        for (var i = 0; i < thermalProfileCurveList.Count(); i++)
        {
            ThermalProfilePoint thermalProfileCurrentPoint = thermalProfileCurveList[i];
            ThermalProfilePoint thermalProfileNextPoint = thermalProfileCurveList[i + 1];

            if ((secondsElapsed + interpolationTimeSeconds) >= thermalProfileNextPoint.SecondsElapsed)
                continue;
            
            //Angular coefficient
            float k = (thermalProfileNextPoint.Temperature - thermalProfileCurrentPoint.Temperature) /
                    (thermalProfileNextPoint.SecondsElapsed - thermalProfileCurrentPoint.SecondsElapsed);

            var delta = secondsElapsed+interpolationTimeSeconds - thermalProfileCurrentPoint.SecondsElapsed;

            return (true, (ushort)(thermalProfileCurrentPoint.Temperature+k * delta));
        }

        return (false, 0);
    }
}