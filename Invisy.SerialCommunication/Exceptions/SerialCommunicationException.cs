namespace Invisy.SerialCommunication.Exceptions;

public class SerialCommunicationException : Exception
{
    public SerialCommunicationException() : base()
    {
    }

    public SerialCommunicationException(string message) : base(message)
    {
    }

    public SerialCommunicationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}