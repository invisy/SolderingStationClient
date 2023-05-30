using System.Runtime.InteropServices;
using SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;

namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct GetCurrentTemperatureCommandParameters(byte PidId);

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct GetCurrentTemperatureCommandResult(ushort Temperature);

public class
    GetCurrentTemperatureCommand : ISelfBuiltDeviceCommand<GetCurrentTemperatureCommandParameters,
        GetCurrentTemperatureCommandResult>
{
    public GetCurrentTemperatureCommand(byte pidId)
    {
        ParamsRequest = new GetCurrentTemperatureCommandParameters
        {
            PidId = pidId
        };
    }

    public ushort Command => (ushort)CommandsEnum.GetCurrentTemperature;

    public GetCurrentTemperatureCommandParameters ParamsRequest { get; }
}