using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Abstractions;

public interface ISerialPortsSettingsService
{
    Task<SerialPortSettings?> GetByPortName(string portName);
    Task Add(SerialPortSettings portSettings);
    Task Remove(string portName);
    Task Update(SerialPortSettings portSettings);
}