namespace SolderingStationClient.Models.TemperatureControllers;

public class TemperatureControllerData
{
    public TemperatureControllerData(IReadOnlyCollection<float> temperatureHistory, float desiredTemperature)
    {
        TemperatureHistory = temperatureHistory;
        DesiredTemperature = desiredTemperature;
    }

    public IReadOnlyCollection<float> TemperatureHistory { get; }
    public float DesiredTemperature { get; }
}