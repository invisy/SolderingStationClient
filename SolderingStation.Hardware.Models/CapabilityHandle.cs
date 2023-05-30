namespace SolderingStation.Hardware.Models;

public class CapabilityHandle<TCapabilityHandle>
{
    public CapabilityHandle(ulong deviceId, TCapabilityHandle handle)
    {
        DeviceId = deviceId;
        Handle = handle;
    }

    public ulong DeviceId { get; }
    public TCapabilityHandle Handle { get; }
}