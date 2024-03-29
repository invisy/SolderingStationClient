﻿using System.Runtime.InteropServices;
using SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;

namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SetKpCoefficientCommandParameters
{
    public byte PidId { get; init; }
    public float Kp { get; init; }
}

public class SetKpCoefficientCommand : ISelfBuiltDeviceCommand<SetKpCoefficientCommandParameters>
{
    public SetKpCoefficientCommand(byte pidId, float kp)
    {
        ParamsRequest = new SetKpCoefficientCommandParameters
        {
            PidId = pidId,
            Kp = kp
        };
    }

    public ushort Command => (ushort)CommandsEnum.SetKpCoefficient;
    public SetKpCoefficientCommandParameters ParamsRequest { get; }
}