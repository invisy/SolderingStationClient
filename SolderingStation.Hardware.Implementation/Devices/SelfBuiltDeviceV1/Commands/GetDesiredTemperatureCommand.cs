using System.Runtime.InteropServices;
using SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;

namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct GetDesiredTemperatureCommandParameters
{
    public byte PidId { get; init; }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct GetDesiredTemperatureCommandResult
{
    public Q15 Temperature { get; }
}

public class GetDesiredTemperatureCommand : ISelfBuiltDeviceCommand<GetDesiredTemperatureCommandParameters
    ,
    GetDesiredTemperatureCommandResult>
{
    public GetDesiredTemperatureCommand(byte pidId)
    {
        ParamsRequest = new GetDesiredTemperatureCommandParameters
        {
            PidId = pidId
        };
    }

    public ushort Command => (ushort)CommandsEnum.GetExpectedTemperature;
    public GetDesiredTemperatureCommandParameters ParamsRequest { get; }
}