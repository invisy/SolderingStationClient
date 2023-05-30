namespace SolderingStationClient.Models;

public class DeviceConnectedEventArgs : EventArgs
{
    public DeviceConnectedEventArgs(Device device)
    {
        Device = device;
    }

    public Device Device { get; }
}