using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.Models;

public record ThermalProfile(uint Id, string Name, IEnumerable<ControllerThermalProfile> ControllersThermalProfiles);