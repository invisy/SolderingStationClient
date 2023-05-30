namespace SolderingStationClient.Models;

public class SerialPortInfo
{
    public SerialPortInfo(string serialPortName, ulong connectedDeviceId)
    {
        SerialPortName = serialPortName;
        ConnectedDeviceId = connectedDeviceId;
    }

    public string SerialPortName { get; }
    public ulong ConnectedDeviceId { get; set; }
}