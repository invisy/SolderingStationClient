using System.Runtime.InteropServices;
using SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;

namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct GetPidsNumberCommandParameters
{
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct GetPidsNumberCommandResult
{
    public byte ChannelsNumber { get; }
}

public class GetChannelsNumberCommand : ISelfBuiltDeviceCommand<GetPidsNumberCommandParameters,
    GetPidsNumberCommandResult>
{
    public ushort Command => (ushort)CommandsEnum.GetPidsNumber;
    public GetPidsNumberCommandParameters ParamsRequest { get; } = default;
}