namespace SolderingStationClient.Models.TemperatureControllers;

public record TemperatureMeasurementPoint(uint Id, float SecondsElapsed, ushort Temperature);