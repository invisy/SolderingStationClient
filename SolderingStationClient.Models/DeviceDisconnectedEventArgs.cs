namespace SolderingStationClient.Models;

public class DeviceDisconnectedEventArgs : EventArgs
{
    public DeviceDisconnectedEventArgs(ulong deviceId)
    {
        DeviceId = deviceId;
    }

    public ulong DeviceId { get; }
}