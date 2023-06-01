namespace SolderingStationClient.BLL.Implementation.Exceptions;

public class ServiceException : Exception
{
    public ServiceException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
    
    public ServiceException(string? message) : base(message)
    {
    }
    
    public ServiceException()
    {
    }
}