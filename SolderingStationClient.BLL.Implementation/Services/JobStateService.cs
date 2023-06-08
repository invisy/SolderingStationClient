using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.BLL.Implementation.Exceptions;
using SolderingStationClient.BLL.Implementation.Extensions;
using SolderingStationClient.Models.Jobs;

namespace SolderingStationClient.BLL.Implementation.Services;

public class JobStateService : IJobStateService
{
    private bool _isDisposed;
    private IJob? _activeJob;

    public event EventHandler<JobStartedEventArgs> JobStarted;

    public void AddJob(IJob job)
    {
        if (_activeJob != null)
            throw new JobException("You can`t run second job at the same time!");
        
        SubscribeToJobEvents(job);

        _activeJob = job;
        JobStarted?.Invoke(this, new JobStartedEventArgs(job));
    }

    private void SubscribeToJobEvents(IJob job)
    {
        job.StateChanged += JobOnStateChanged;
    }

    private void UnsubscribeFromJobEvents(IJob job)
    {
        job.StateChanged -= JobOnStateChanged;
    }

    private void JobOnStateChanged(object? sender, JobStateChangedEventArgs e)
    {
        if (sender is null)
            return;

        var job = (IJob)sender;
        if (!e.JobState.IsCompleted())
            return;

        _activeJob = null;
        UnsubscribeFromJobEvents(job);
    }
    
    public void Dispose()
    {
        Dispose(true);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing && _activeJob != null)
            UnsubscribeFromJobEvents(_activeJob);
            
        _isDisposed = true;
    }
}