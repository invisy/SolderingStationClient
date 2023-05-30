using Invisy.SerialCommunication.Models;
using Invisy.SerialCommunication.Utils;

namespace Invisy.SerialCommunication;

public class SerialCommunicationClient : ISerialCommunicationClient
{
    private const int MaxDataLength = 250;
    private readonly ICrc16Calculator _crc16Calculator;
    private readonly object _lock = new();
    private readonly int _responseTimeout;
    private readonly IStructSerializer _serializer;
    private readonly ISerialPort _serialPort;

    private bool _disposedValue;

    public SerialCommunicationClient(ISerialPort serialPort, IStructSerializer serializer,
        ICrc16Calculator crc16Calculator,
        SerialPortSettings settings, int responseTimeout = 500)
    {
        _serializer = serializer;
        _serialPort = serialPort;
        _crc16Calculator = crc16Calculator;
        _responseTimeout = responseTimeout;
        PortSettings = settings;

        _serialPort.PortName = PortSettings.PortName;
        _serialPort.BaudRate = PortSettings.BaudRate;
        _serialPort.Parity = PortSettings.Parity;
        _serialPort.DataBits = PortSettings.DataBits;
        _serialPort.StopBits = PortSettings.StopBits;
    }

    public SerialPortSettings PortSettings { get; }

    public ConnectionResult Connect()
    {
        if (_serialPort.IsOpen)
            return ConnectionResult.PortIsBusy;

        try
        {
            _serialPort.Open();
            return ConnectionResult.Ok;
        }
        catch (FileNotFoundException)
        {
            return ConnectionResult.PortIsNotFound;
        }
        catch (Exception)
        {
            return ConnectionResult.PortIsBusy;
        }
    }

    public void Close()
    {
        Dispose();
    }

    public CommandExecutionResult ExecuteCommand<TParameters>(ushort commandCode, TParameters parameters)
        where TParameters : struct
    {
        var commandParams = _serializer.Serialize(parameters);
        var byteResult = ExecuteCommand(commandCode, commandParams);
        if (byteResult.Status == CommandExecutionStatus.Ok && byteResult.Data.Length == 0)
            return new CommandExecutionResult(CommandExecutionStatus.Ok);

        return new CommandExecutionResult(CommandExecutionStatus.UnexpectedResponse);
    }

    public CommandExecutionResult<TResult> ExecuteCommand<TParameters, TResult>(ushort commandCode,
        TParameters parameters)
        where TParameters : struct where TResult : struct
    {
        var commandParams = _serializer.Serialize(parameters);
        var byteResult = ExecuteCommand(commandCode, commandParams);

        var response = _serializer.Deserialize<TResult>(byteResult.Data);
        return response != null
            ? new CommandExecutionResult<TResult>(byteResult.Status, response.Value)
            : new CommandExecutionResult<TResult>(CommandExecutionStatus.UnexpectedResponse, new TResult());
    }

    public CommandExecutionResult<byte[]> ExecuteCommand(ushort command, byte[] parameters)
    {
        try
        {
            lock (_lock)
            {
                SendPacket(command, parameters);
                var result = ReceivePacket();

                return result;
            }
        }
        catch (Exception ex) when (ex is InvalidOperationException or TimeoutException)
        {
            return new CommandExecutionResult<byte[]>(CommandExecutionStatus.PortError, Array.Empty<byte>());
        }
    }

    public async  Task<CommandExecutionResult> ExecuteCommandAsync<TParameters>(ushort commandCode, TParameters parameters)
        where TParameters : struct
    {
        return await Task.Run(() => ExecuteCommand(commandCode, parameters));
    }

    public async Task<CommandExecutionResult<TResult>> ExecuteCommandAsync<TParameters, TResult>(ushort commandCode,
        TParameters parameters)
        where TParameters : struct where TResult : struct
    {
        return await Task.Run(() => ExecuteCommand<TParameters, TResult>(commandCode, parameters));
    }

    public async Task<CommandExecutionResult<byte[]>> ExecuteCommandAsync(ushort command, byte[] parameters)
    {
        return await Task.Run(() => ExecuteCommand(command, parameters));
    }

    public void Dispose()
    {
        if (!_disposedValue)
        {
            _serialPort.Dispose();
            _disposedValue = true;
            GC.SuppressFinalize(this);
        }
    }

    private void SendPacket(ushort command, byte[] parameters)
    {
        if (parameters is null)
            throw new ArgumentNullException(nameof(parameters));

        var dataLength = parameters.Length;

        if (dataLength > MaxDataLength)
            throw new ArgumentException($"Command parameters length can`t be more than {MaxDataLength} bytes",
                nameof(parameters));

        var headerLength = 3;
        var crcLength = 2;
        var packet = new byte[headerLength + dataLength + crcLength]; // header(3 bytes) + data + crc(2bytes)

        packet[0] = (byte)(command & 0xff);
        packet[1] = (byte)((command >> 8) & 0xff);
        packet[2] = (byte)dataLength;
        Array.Copy(parameters, 0, packet, 3, parameters.Length);
        var crc = _crc16Calculator.Calculate(packet, 0, 3 + dataLength);
        packet[^2] = (byte)(crc & 0xff);
        packet[^1] = (byte)((crc >> 8) & 0xff);

        _serialPort.Write(packet, 0, packet.Length);
        while (_serialPort.BytesToWrite > 0)
            Thread.Sleep(3);
    }

    private CommandExecutionResult<byte[]> ReceivePacket()
    {
        var buffer = new byte[256];
        var headerLength = 2;
        var crcLength = 2;

        var isAnyResponse = IsAnyResponse();
        if (!isAnyResponse)
            return new CommandExecutionResult<byte[]>(CommandExecutionStatus.NoResponse, Array.Empty<byte>());

        var isHeaderInBuffer = IsDataInBuffer(headerLength);
        if (!isHeaderInBuffer)
            return new CommandExecutionResult<byte[]>(CommandExecutionStatus.PackageIsCorrupted, Array.Empty<byte>());

        _serialPort.Read(buffer, 0, headerLength); // read header

        var dataLength = buffer[headerLength - 1];
        if (dataLength > MaxDataLength)
            return new CommandExecutionResult<byte[]>(CommandExecutionStatus.PackageIsCorrupted, Array.Empty<byte>());

        var dataAndChecksumLength = dataLength + crcLength;
        var isFullPacketInBuffer = IsDataInBuffer(dataAndChecksumLength);
        if (!isFullPacketInBuffer)
            return new CommandExecutionResult<byte[]>(CommandExecutionStatus.PackageIsCorrupted, Array.Empty<byte>());
        _serialPort.Read(buffer, 2, dataAndChecksumLength); // read to end of packet

        var fullPacketLength = headerLength + dataAndChecksumLength;

        var calculatedCrc = _crc16Calculator.Calculate(buffer, 0, headerLength + dataLength);
        var receivedCrc = buffer[fullPacketLength - 2] | (buffer[fullPacketLength - 1] << 8);
        if (calculatedCrc != receivedCrc)
            return new CommandExecutionResult<byte[]>(CommandExecutionStatus.PackageIsCorrupted, Array.Empty<byte>());

        if (buffer[0] != (byte)CommandExecutionStatus.Ok)
            return new CommandExecutionResult<byte[]>((CommandExecutionStatus)buffer[0], Array.Empty<byte>());

        var resultData = new byte[dataLength];
        Array.Copy(buffer, headerLength, resultData, 0, dataLength);
        return new CommandExecutionResult<byte[]>(CommandExecutionStatus.Ok, resultData);
    }

    private bool IsAnyResponse()
    {
        short timer = 0;
        while (_serialPort.BytesToRead == 0)
        {
            Thread.Sleep(1);
            timer++;
            if (timer >= _responseTimeout)
                return false;
        }

        return true;
    }

    private bool IsDataInBuffer(int bytesNumber)
    {
        var bytesAvailable = _serialPort.BytesToRead;
        while (bytesAvailable < bytesNumber)
        {
            var symbolTime = Math.Max(1000 / (_serialPort.BaudRate / 8) * 4, 2);
            //delay == transfer speed of 1 byte multiplied by 4 if it`s more than 2ms
            Thread.Sleep(symbolTime);
            var newBytesAvailable = _serialPort.BytesToRead;
            if (bytesAvailable == newBytesAvailable)
                return false;

            bytesAvailable = newBytesAvailable;
        }

        return true;
    }
}