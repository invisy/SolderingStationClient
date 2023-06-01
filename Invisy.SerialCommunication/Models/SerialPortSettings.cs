using System.IO.Ports;

namespace Invisy.SerialCommunication.Models;

public class SerialPortSettings
{
    public string PortName { get; init; } = string.Empty;
    public int BaudRate { get; init; }
    public Parity Parity { get; init; }
    public int DataBits { get; init; }
    public StopBits StopBits { get; init; }
}