namespace SolderingStation.Hardware.Models;

public class DesiredTemperatureChangedEventArgs : EventArgs
{
    public DesiredTemperatureChangedEventArgs(byte channel, ushort desiredTemperature)
    {
        Channel = channel;
        DesiredTemperature = desiredTemperature;
    }

    public byte Channel { get; }
    public ushort DesiredTemperature { get; }
}