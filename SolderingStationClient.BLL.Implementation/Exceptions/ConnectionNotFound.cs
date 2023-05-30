namespace SolderingStationClient.BLL.Implementation.Exceptions;

public class ConnectionNotFound : ConnectionException
{
    public ConnectionNotFound()
    {
    }

    public ConnectionNotFound(string message, Exception innerException) : base(message, innerException)
    {
    }
}