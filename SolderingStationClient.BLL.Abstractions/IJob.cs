using SolderingStationClient.Models.Jobs;

namespace SolderingStationClient.BLL.Abstractions;

public interface IJob
{
    JobType JobType { get; }
    JobState State { get; }
    float CurrentProgress { get; }

    event EventHandler<JobStateChangedEventArgs> StateChanged;
    event EventHandler<JobProgressUpdatedEventArgs> ProgressUpdated;

    Task RunAsync();
    void Cancel();
}