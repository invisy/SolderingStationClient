namespace SolderingStationClient.BLL.Implementation.Exceptions;

public class CapabilityIsNotSupportedException : Exception
{
    public CapabilityIsNotSupportedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
    
    public CapabilityIsNotSupportedException(string? message) : base(message)
    {
    }
    
    public CapabilityIsNotSupportedException()
    {
    }
}