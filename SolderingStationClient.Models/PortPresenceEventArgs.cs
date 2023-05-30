using SolderingStation.Hardware.Models;

namespace SolderingStationClient.Models;

public class SerialPortPresenceEventArgs : EventArgs
{
    public SerialPortPresenceEventArgs(string portName, PresenceStatus status)
    {
        PortName = portName;
        Status = status;
    }

    public string PortName { get; }
    public PresenceStatus Status { get; }
}