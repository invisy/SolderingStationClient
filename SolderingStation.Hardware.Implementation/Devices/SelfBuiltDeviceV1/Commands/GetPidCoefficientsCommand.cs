using System.Runtime.InteropServices;
using SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;

namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct GetPidCoefficientsCommandParameters
{
    public byte PidId { get; init; }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct GetPidCoefficientsCommandResult
{
    public float Kp { get; }
    public float Ki { get; }
    public float Kd { get; }
}

public class
    GetPidCoefficientsCommand : ISelfBuiltDeviceCommand<GetPidCoefficientsCommandParameters,
        GetPidCoefficientsCommandResult>
{
    public GetPidCoefficientsCommand(byte pidId)
    {
        ParamsRequest = new GetPidCoefficientsCommandParameters
        {
            PidId = pidId
        };
    }

    public ushort Command => (ushort)CommandsEnum.GetPidCoefficients;
    public GetPidCoefficientsCommandParameters ParamsRequest { get; }
}