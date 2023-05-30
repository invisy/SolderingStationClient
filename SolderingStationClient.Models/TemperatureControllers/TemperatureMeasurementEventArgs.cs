namespace SolderingStationClient.Models.TemperatureControllers;

public class TemperatureMeasurementEventArgs : EventArgs
{
    public TemperatureMeasurementEventArgs(TemperatureControllerKey temperatureControllerId,
        TemperatureMeasurement temperature, ushort desiredTemperature)
    {
        TemperatureControllerId = temperatureControllerId;
        Temperature = temperature;
        DesiredTemperature = desiredTemperature;
    }

    public TemperatureControllerKey TemperatureControllerId { get; }
    public TemperatureMeasurement Temperature { get; }
    public ushort DesiredTemperature { get; }
}