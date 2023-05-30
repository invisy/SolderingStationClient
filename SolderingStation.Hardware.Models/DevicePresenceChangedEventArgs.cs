namespace SolderingStation.Hardware.Models;

public class DevicePresenceChangedEventArgs<T> : EventArgs
{
    public DevicePresenceChangedEventArgs(T deviceId, PresenceStatus status)
    {
        DeviceId = deviceId;
        Status = status;
    }

    public T DeviceId { get; }
    public PresenceStatus Status { get; }
}