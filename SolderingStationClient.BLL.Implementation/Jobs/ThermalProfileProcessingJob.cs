using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.Models.Jobs;

namespace SolderingStationClient.BLL.Implementation.Jobs;

public class ThermalProfileProcessingJob : IThermalProfileProcessingJob
{
    private CancellationTokenSource _tokenSource = new();
    private JobState _state = JobState.NotStarted;

    public JobType JobType => JobType.ThermalProfileProcessing;

    public JobState State
    {
        get => _state;
        private set
        {
            _state = value;
            StateChanged?.Invoke(this, new JobStateChangedEventArgs(_state));
        }
    }

    private float _currentProgress = 0;

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
        State = JobState.Stopped;
    }

    public void Cancel()
    {
        _tokenSource.Cancel();
    }

    private async Task Processing()
    {
        await Task.Delay(1000);
        CurrentProgress = 33;
        await Task.Delay(1000);
        CurrentProgress = 70;
        await Task.Delay(1000);
        CurrentProgress = 100;
    }
}