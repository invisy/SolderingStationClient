using SolderingStationClient.Models;
using SolderingStationClient.Models.Dto;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface ISerialPortsService : IDisposable
{
    IEnumerable<SerialPortInfoDto> SerialPorts { get; }
    event EventHandler<SerialPortInfoEventArgs> PortAdded;
    event EventHandler<SerialPortRemovedEventArgs> PortRemoved;
    event EventHandler<SerialPortInfoEventArgs> PortInfoUpdateEvent;

    Task Connect(string portName);
    void Disconnect(string portName);
}