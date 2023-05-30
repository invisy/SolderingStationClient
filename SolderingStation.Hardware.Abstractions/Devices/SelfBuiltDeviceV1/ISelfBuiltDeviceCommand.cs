namespace SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;

public interface ISelfBuiltDeviceCommand<TRequest> where TRequest : struct
{
    ushort Command { get; }
    TRequest ParamsRequest { get; }
}

public interface ISelfBuiltDeviceCommand<TRequest, TResponse> : ISelfBuiltDeviceCommand<TRequest>
    where TRequest : struct where TResponse : struct
{
}