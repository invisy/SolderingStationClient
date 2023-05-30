namespace SolderingStationClient.Models;

public class SerialPortRemovedEventArgs : EventArgs
{
    public SerialPortRemovedEventArgs(string portName)
    {
        PortName = portName;
    }

    public string PortName { get; }
}