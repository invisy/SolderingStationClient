namespace SolderingStationClient.BLL.Implementation.Exceptions;

public class JobException : Exception
{
    public JobException()
    {
    }

    public JobException(string message) : base(message)
    {
    }
}