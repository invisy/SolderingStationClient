using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Abstractions;

public interface ISerialPortsSettingsService
{
    SerialPortSettings? GetByPortName(string portName);
    void Add(SerialPortSettings portSettings);
    void Remove(string portName);
    void Update(SerialPortSettings portSettings);
}