namespace SolderingStationClient.Models.Jobs;

public class JobStateChangedEventArgs : EventArgs
{
    public JobStateChangedEventArgs(JobState jobState)
    {
        JobState = jobState;
    }

    public JobState JobState { get; }
}