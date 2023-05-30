namespace SolderingStationClient.Models.TemperatureControllers;

public record ThermalProfilePoint(uint Id, float SecondsElapsed, ushort Temperature);