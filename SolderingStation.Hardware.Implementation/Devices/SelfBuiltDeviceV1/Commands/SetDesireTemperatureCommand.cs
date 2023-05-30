using System.Runtime.InteropServices;
using SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;

namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SetDesireTemperatureCommandParameters
{
    public byte PidId { get; init; }
    public ushort Temperature { get; init; }
}

public class SetDesireTemperatureCommand : ISelfBuiltDeviceCommand<SetDesireTemperatureCommandParameters>
{
    public SetDesireTemperatureCommand(byte pidId, ushort temperature)
    {
        ParamsRequest = new SetDesireTemperatureCommandParameters
        {
            PidId = pidId,
            Temperature = temperature
        };
    }

    public ushort Command => (ushort)CommandsEnum.SetExpectedTemperature;
    public SetDesireTemperatureCommandParameters ParamsRequest { get; }
}