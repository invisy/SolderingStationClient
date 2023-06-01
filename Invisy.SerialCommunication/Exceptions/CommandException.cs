namespace Invisy.SerialCommunication.Exceptions;

public class CommandException : SerialCommunicationException
{
    public CommandException() : base()
    {
    }

    public CommandException(string message) : base(message)
    {
    }

    public CommandException(string message, Exception innerException) : base(message, innerException)
    {
    }
}