namespace SolderingStationClient.Models.Jobs;

public class JobProgressUpdatedEventArgs : EventArgs
{
    public JobProgressUpdatedEventArgs(float currentProgress)
    {
        CurrentProgress = currentProgress;
    }

    public float CurrentProgress { get; }
}