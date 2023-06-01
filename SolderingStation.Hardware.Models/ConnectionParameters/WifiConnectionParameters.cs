namespace SolderingStation.Hardware.Models.ConnectionParameters;

public record WifiConnectionParameters(string ip) : IConnectionParameters;