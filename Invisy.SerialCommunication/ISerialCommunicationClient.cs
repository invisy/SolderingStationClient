using Invisy.SerialCommunication.Models;

namespace Invisy.SerialCommunication;

public interface ISerialCommunicationClient : IDisposable
{
    SerialPortSettings PortSettings { get; }
    public ConnectionResult Connect();
    public void Close();
    public CommandExecutionResult<byte[]> ExecuteCommand(ushort commandCode, byte[] parameters);

    public CommandExecutionResult ExecuteCommand<TParameters>(ushort commandCode, TParameters parameters)
        where TParameters : struct;

    public CommandExecutionResult<TResult> ExecuteCommand<TParameters, TResult>(ushort commandCode,
        TParameters parameters)
        where TParameters : struct where TResult : struct;

    public Task<CommandExecutionResult<byte[]>> ExecuteCommandAsync(ushort commandCode, byte[] parameters);

    public Task<CommandExecutionResult> ExecuteCommandAsync<TParameters>(ushort commandCode, TParameters parameters)
        where TParameters : struct;

    public Task<CommandExecutionResult<TResult>> ExecuteCommandAsync<TParameters, TResult>(ushort commandCode,
        TParameters parameters)
        where TParameters : struct where TResult : struct;
}