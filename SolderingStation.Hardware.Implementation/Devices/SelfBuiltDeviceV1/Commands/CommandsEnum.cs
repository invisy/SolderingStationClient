namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Commands;

public enum CommandsEnum : ushort
{
    GetPidsNumber = 1,
    GetCurrentTemperature,
    GetExpectedTemperature,
    GetPidCoefficients,
    SetExpectedTemperature,
    SetKpCoefficient,
    SetKiCoefficient,
    SetKdCoefficient
}