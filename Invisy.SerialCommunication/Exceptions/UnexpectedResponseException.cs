namespace Invisy.SerialCommunication.Exceptions;

public class UnexpectedResponseException : ConnectionException
{
    public UnexpectedResponseException() : base()
    {
    }

    public UnexpectedResponseException(string message) : base(message)
    {
    }

    public UnexpectedResponseException(string message, Exception innerException) : base(message, innerException)
    {
    }
}