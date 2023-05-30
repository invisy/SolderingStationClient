using System.IO.Ports;
using SolderingStationClient.BLL.Abstractions;

namespace SolderingStationClient.BLL.Implementation;

public class SerialPortsProvider : ISerialPortsProvider
{
    public IEnumerable<string> GetAvailablePorts()
    {
        return SerialPort.GetPortNames();
    }
}