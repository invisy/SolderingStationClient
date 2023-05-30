namespace SolderingStation.Hardware.Abstractions.Connections;

public interface IWirelessConnectionCapability : IConnectionCapability
{
    int GetSignalStrength();
}