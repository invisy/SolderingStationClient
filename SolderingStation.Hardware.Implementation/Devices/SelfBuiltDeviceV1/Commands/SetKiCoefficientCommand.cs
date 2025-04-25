using System.Runtime.InteropServices;
using SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;

namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SetKiCoefficientCommandParameters
{
    public byte PidId { get; init; }
    public Q15 Ki { get; init; }
}

public class SetKiCoefficientCommand : ISelfBuiltDeviceCommand<SetKiCoefficientCommandParameters>
{
    public SetKiCoefficientCommand(byte pidId, Q15 ki)
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