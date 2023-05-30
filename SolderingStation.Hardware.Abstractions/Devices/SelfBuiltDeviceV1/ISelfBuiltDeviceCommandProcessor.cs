namespace SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;

public interface ISelfBuiltDeviceCommandProcessor
{
    Task ExecuteCommand<TRequest>(ISelfBuiltDeviceCommand<TRequest> command) where TRequest : struct;

    Task<TResponse> ExecuteCommand<TRequest, TResponse>(
        ISelfBuiltDeviceCommand<TRequest, TResponse> command)
        where TRequest : struct where TResponse : struct;
}