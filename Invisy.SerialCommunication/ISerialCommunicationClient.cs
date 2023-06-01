using Invisy.SerialCommunication.Models;

namespace Invisy.SerialCommunication;

public interface ISerialCommunicationClient : IDisposable
{
    SerialPortSettings PortSettings { get; }
    public void Connect();
    public void Close();
    
    public byte[] ExecuteCommand(ushort commandCode, byte[] parameters);
    public void ExecuteCommand<TParameters>(ushort commandCode, TParameters parameters)
        where TParameters : struct;
    public TResult ExecuteCommand<TParameters, TResult>(ushort commandCode,
        TParameters parameters)
        where TParameters : struct where TResult : struct;

    public Task<byte[]> ExecuteCommandAsync(ushort commandCode, byte[] parameters);
    public Task ExecuteCommandAsync<TParameters>(ushort commandCode, TParameters parameters)
        where TParameters : struct;
    public Task<TResult> ExecuteCommandAsync<TParameters, TResult>(ushort commandCode,
        TParameters parameters)
        where TParameters : struct where TResult : struct;
}