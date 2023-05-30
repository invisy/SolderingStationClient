namespace SolderingStationClient.Models.Jobs;

public class JobStartedEventArgs
{
    public JobStartedEventArgs(JobType jobType)
    {
        JobType = jobType;
    }

    public JobType JobType { get; }
}