namespace SolderingStationClient.Models.Jobs;

public interface IJob
{
    JobState State { get; }
    float CurrentProgress { get; }

    event EventHandler<JobStateChangedEventArgs> StateChanged;
    event EventHandler<JobProgressUpdatedEventArgs> ProgressUpdated;

    Task RunAsync();
    void Cancel();
}