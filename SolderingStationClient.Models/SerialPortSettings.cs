using System.IO.Ports;

namespace SolderingStationClient.Models;

public class SerialPortSettings
{
    public string PortName { get; }
    public int BaudRate { get; } = 9600;
    public Parity Parity { get; } = Parity.None;
    public int DataBits { get; } = 8;
    public StopBits StopBits { get; } = StopBits.One;

    public SerialPortSettings(string portName)
    {
        PortName = portName;
    }
    
    public SerialPortSettings(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
    {
        PortName = portName;
        BaudRate = baudRate;
        Parity = parity;
        DataBits = dataBits;
        StopBits = stopBits;
    }
}