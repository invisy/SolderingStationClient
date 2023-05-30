using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.Models;

public record ControllerThermalProfile(uint Id, string Name, int ArgbColor, IEnumerable<TemperatureMeasurement> TemperatureMeasurements);