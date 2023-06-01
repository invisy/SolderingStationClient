namespace Invisy.SerialCommunication.Exceptions;

public class PackageIsCorruptedException : CommandException
{
    public PackageIsCorruptedException() : base()
    {
    }

    public PackageIsCorruptedException(string message) : base(message)
    {
    }

    public PackageIsCorruptedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}