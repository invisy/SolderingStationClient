using SolderingStationClient.Models.Jobs;

namespace SolderingStationClient.BLL.Implementation.Extensions;

public static class JobExtensions
{
    public static bool IsCompleted(this JobState jobState)
    {
        var completedOperationStates = new[]
        {
            JobState.Failed,
            JobState.Finished,
            JobState.Stopped
        };

        return completedOperationStates.Contains(jobState);
    }
}