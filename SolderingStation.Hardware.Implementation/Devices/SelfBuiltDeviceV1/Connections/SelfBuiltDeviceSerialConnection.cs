using Invisy.SerialCommunication;
using Invisy.SerialCommunication.Models;
using SolderingStation.Hardware.Abstractions.Connections;
using SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;
using SolderingStation.Hardware.Models.ConnectionParameters;

namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Connections;

public class SelfBuiltDeviceSerialConnection : IConnectionGeneric<SerialConnectionParameters>,
    ISelfBuiltDeviceCommandProcessor
{
    private readonly ISerialCommunicationClient _client;
    private bool _disposed;

    public SelfBuiltDeviceSerialConnection(ISerialCommunicationClient client,
        SerialConnectionParameters connectionParameters)
    {
        ConnectionParameters = connectionParameters;
        _client = client;
    }

    public SerialConnectionParameters ConnectionParameters { get; }

    public bool Connect()
    {
        var result = _client.Connect();

        return result == ConnectionResult.Ok;
    }

    public void Disconnect()
    {
        _client.Close();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        Disconnect();
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public async Task ExecuteCommand<TRequest>(ISelfBuiltDeviceCommand<TRequest> command)
        where TRequest : struct
    {
        await _client.ExecuteCommandAsync(command.Command, command.ParamsRequest);
    }

    public async Task<TResponse> ExecuteCommand<TRequest, TResponse>(
        ISelfBuiltDeviceCommand<TRequest, TResponse> command)
        where TRequest : struct where TResponse : struct
    {
        var result = await _client.ExecuteCommandAsync<TRequest, TResponse>(command.Command, command.ParamsRequest);

        if (result.Status != CommandExecutionStatus.Ok)
            throw new Exception();

        return result.Data;
    }
}