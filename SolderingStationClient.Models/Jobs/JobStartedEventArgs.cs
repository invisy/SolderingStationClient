namespace SolderingStationClient.Models.Jobs;

public class JobStartedEventArgs
{
    public JobStartedEventArgs(IJob job)
    {
        Job = job;
    }

    public IJob Job { get; }
}