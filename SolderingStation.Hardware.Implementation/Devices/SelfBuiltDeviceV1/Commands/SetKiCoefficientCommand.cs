using System.Runtime.InteropServices;
using SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;

namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SetKiCoefficientCommandParameters
{
    public byte PidId { get; init; }
    public float Ki { get; init; }
}

public class SetKiCoefficientCommand : ISelfBuiltDeviceCommand<SetKiCoefficientCommandParameters>
{
    public SetKiCoefficientCommand(byte pidId, float ki)
    {
        ParamsRequest = new SetKiCoefficientCommandParameters
        {
            PidId = pidId,
            Ki = ki
        };
    }

    public ushort Command => (ushort)CommandsEnum.SetKiCoefficient;
    public SetKiCoefficientCommandParameters ParamsRequest { get; }
}