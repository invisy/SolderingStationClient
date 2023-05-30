using SolderingStation.Hardware.Abstractions.Connections;
using SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;
using SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Commands;
using SolderingStation.Hardware.Models;
using SolderingStation.Hardware.Models.ConnectionParameters;

namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1;

public class
    SelfBuiltDeviceV1<TConnectionParameters, TConnection> : ISelfBuiltDeviceV1<TConnectionParameters, TConnection>
    where TConnectionParameters : IConnectionParameters
    where TConnection : IConnectionGeneric<TConnectionParameters>, ISelfBuiltDeviceCommandProcessor
{
    private readonly TConnection _connection;
    private bool _disposed;

    public IConnection Connection => _connection;

    public SelfBuiltDeviceV1(TConnection connection)
    {
        _connection = connection;
    }

    public string Name => "Soldering Station V1";

    public bool Connect()
    {
        return _connection.Connect();
    }

    public virtual async Task<bool> Probe()
    {
        try
        {
            var channels = await GetChannelsNumber();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public void CloseConnection()
    {
        _connection.Disconnect();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _connection.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public async Task<byte> GetChannelsNumber()
    {
        var command = new GetChannelsNumberCommand();
        var result = await _connection.ExecuteCommand(command);

        return result.ChannelsNumber;
    }

    public async Task<ushort> GetCurrentTemperature(byte channel)
    {
        var command = new GetCurrentTemperatureCommand(channel);
        var result = await _connection.ExecuteCommand(command);

        return result.Temperature;
    }

    public async Task<ushort> GetDesiredTemperature(byte channel)
    {
        var command = new GetDesiredTemperatureCommand(channel);
        var result = await _connection.ExecuteCommand(command);

        return result.Temperature;
    }

    public async Task SetDesiredTemperature(byte channel, ushort temperature)
    {
        var command = new SetDesireTemperatureCommand(channel, temperature);
        await _connection.ExecuteCommand(command);
    }

    public async Task<PidCoefficients> GetCoefficients(byte channel)
    {
        var command = new GetPidCoefficientsCommand(channel);
        var commandResult = await _connection.ExecuteCommand(command);

        var result = new PidCoefficients(commandResult.Kp, commandResult.Ki, commandResult.Kd);

        return result;
    }

    public async Task SetKpCoefficient(byte channel, float coefficient)
    {
        var command = new SetKpCoefficientCommand(channel, coefficient);
        await _connection.ExecuteCommand(command);
    }

    public async Task SetKiCoefficient(byte channel, float coefficient)
    {
        var command = new SetKiCoefficientCommand(channel, coefficient);
        await _connection.ExecuteCommand(command);
    }

    public async Task SetKdCoefficient(byte channel, float coefficient)
    {
        var command = new SetKdCoefficientCommand(channel, coefficient);
        await _connection.ExecuteCommand(command);
    }
}