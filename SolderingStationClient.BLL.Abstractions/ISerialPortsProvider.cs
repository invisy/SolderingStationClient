namespace SolderingStationClient.BLL.Abstractions;

public interface ISerialPortsProvider
{
    IEnumerable<string> GetAvailablePorts();
}