using SolderingStationClient.Models.Jobs;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface IJobStateService : IDisposable
{
    public event EventHandler<JobStartedEventArgs> JobStarted;
    public void AddJob(IJob job);
}