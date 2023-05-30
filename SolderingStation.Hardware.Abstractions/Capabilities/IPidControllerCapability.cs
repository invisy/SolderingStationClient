using SolderingStation.Hardware.Models;

namespace SolderingStation.Hardware.Abstractions.Capabilities;

public interface IPidControllerCapability : ITemperatureControllerCapability
{
    Task<PidCoefficients> GetCoefficients(byte channel);
    Task SetKdCoefficient(byte channel, float coefficient);
    Task SetKpCoefficient(byte channel, float coefficient);
    Task SetKiCoefficient(byte channel, float coefficient);
}