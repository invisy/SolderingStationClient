using SolderingStation.Hardware.Models.ConnectionParameters;
using SolderingStationClient.Models;
using SolderingStationClient.Models.Dto;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface ISerialPortsService
{
    IEnumerable<SerialPortInfoDto> SerialPorts { get; }
    event EventHandler<SerialPortInfoEventArgs> PortAdded;
    event EventHandler<SerialPortRemovedEventArgs> PortRemoved;
    event EventHandler<SerialPortInfoEventArgs> PortInfoUpdateEvent;

    Task Connect(SerialConnectionParameters connectionParameters);
    void Disconnect(string portName);
}