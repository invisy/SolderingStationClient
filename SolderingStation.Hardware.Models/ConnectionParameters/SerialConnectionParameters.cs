using System.IO.Ports;

namespace SolderingStation.Hardware.Models.ConnectionParameters;

public record SerialConnectionParameters
    (string PortName, int BaudRate, Parity Parity, int DataBits, StopBits StopBits) : IConnectionParameters;