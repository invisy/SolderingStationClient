namespace Invisy.SerialCommunication.Exceptions;

public class ConnectionException : SerialCommunicationException
{
    public ConnectionException() : base()
    {
    }

    public ConnectionException(string message) : base(message)
    {
    }

    public ConnectionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}