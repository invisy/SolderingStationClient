using System.IO.Ports;

namespace Invisy.SerialCommunication;

public interface ISerialPort : IDisposable
{
    int BaudRate { get; set; }
    int BytesToRead { get; }
    int BytesToWrite { get; }
    int DataBits { get; set; }
    bool IsOpen { get; }
    Parity Parity { get; set; }
    string PortName { get; set; }
    StopBits StopBits { get; set; }
    void Open();
    int Read(byte[] buffer, int offset, int count);
    void Write(byte[] buffer, int offset, int count);
}