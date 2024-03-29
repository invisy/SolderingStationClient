﻿using System.Runtime.InteropServices;
using SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;

namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SetKdCoefficientCommandParameters
{
    public byte PidId { get; init; }
    public float Kd { get; init; }
}

public class SetKdCoefficientCommand : ISelfBuiltDeviceCommand<SetKdCoefficientCommandParameters>
{
    public SetKdCoefficientCommand(byte pidId, float kd)
    {
        ParamsRequest = new SetKdCoefficientCommandParameters
        {
            PidId = pidId,
            Kd = kd
        };
    }

    public ushort Command => (ushort)CommandsEnum.SetKdCoefficient;
    public SetKdCoefficientCommandParameters ParamsRequest { get; }
}