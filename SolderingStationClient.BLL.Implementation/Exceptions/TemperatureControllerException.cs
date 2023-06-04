namespace SolderingStationClient.BLL.Implementation.Exceptions;

public class TemperatureControllerException : Exception
{
    public TemperatureControllerException()
    {
    }

    public TemperatureControllerException(string message) : base(message)
    {
    }
}