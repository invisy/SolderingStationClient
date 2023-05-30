namespace SolderingStation.Hardware.Abstractions.Connections;

internal interface IWirelessConnectionCapability : IConnectionCapability
{
    int GetSignalStrength();
}