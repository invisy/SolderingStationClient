using SolderingStation.Hardware.Models;

namespace SolderingStation.Hardware.Abstractions.Capabilities;

public interface ITemperatureControllerCapability : IDeviceCapability
{
    Task<byte> GetChannelsNumber();
    Task<ushort> GetCurrentTemperature(byte channel);
    Task<ushort> GetDesiredTemperature(byte channel);
    Task SetDesiredTemperature(byte channel, ushort temperature);
}