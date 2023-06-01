using Invisy.SerialCommunication.Exceptions;
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

    public void Connect()
    {
        if (_serialPort.IsOpen)
            throw new ConnectionException("Serial port is already opened!");

        try
        {
            _serialPort.Open();
        }
        catch (Exception e)
        {
            throw new ConnectionException("Serial port connection can`t be established", e);
        }
    }

    public void Close()
    {
        Dispose();
    }

    public void ExecuteCommand<TParameters>(ushort commandCode, TParameters parameters)
        where TParameters : struct
    {
        var commandParams = _serializer.Serialize(parameters);
        var data = ExecuteCommand(commandCode, commandParams);
        if (data.Length != 0)
            throw new UnexpectedResponseException("Unexpected response. It must not contain any data.");
    }

    public TResult ExecuteCommand<TParameters, TResult>(ushort commandCode,
        TParameters parameters)
        where TParameters : struct where TResult : struct
    {
        var commandParams = _serializer.Serialize(parameters);
        var data = ExecuteCommand(commandCode, commandParams);

        var response = _serializer.Deserialize<TResult>(data);

        if (response == null)
            throw new UnexpectedResponseException("Unexpected response. Data is not present or it does not fits structure.");

        return response.Value;
    }

    public byte[] ExecuteCommand(ushort command, byte[] parameters)
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
            throw new ConnectionException("Connection was lost");
        }
    }

    public async Task ExecuteCommandAsync<TParameters>(ushort commandCode, TParameters parameters)
        where TParameters : struct
    {
        await Task.Run(() => ExecuteCommand(commandCode, parameters));
    }

    public async Task<TResult> ExecuteCommandAsync<TParameters, TResult>(ushort commandCode,
        TParameters parameters)
        where TParameters : struct where TResult : struct
    {
        return await Task.Run(() => ExecuteCommand<TParameters, TResult>(commandCode, parameters));
    }

    public async Task<byte[]> ExecuteCommandAsync(ushort command, byte[] parameters)
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

    private byte[] ReceivePacket()
    {
        var buffer = new byte[256];
        var headerLength = 2;
        var crcLength = 2;

        var isAnyResponse = IsAnyResponse();
        if (!isAnyResponse)
            throw new PackageIsCorruptedException("No response.");

        var isHeaderInBuffer = IsDataInBuffer(headerLength);
        if (!isHeaderInBuffer)
            throw new PackageIsCorruptedException("Header is not fully present in packet");

        _serialPort.Read(buffer, 0, headerLength); // read header

        var dataLength = buffer[headerLength - 1];
        if (dataLength > MaxDataLength)
            throw new PackageIsCorruptedException("The data length value received was too large. Data cannot be processed.");

        var dataAndChecksumLength = dataLength + crcLength;
        var isFullPacketInBuffer = IsDataInBuffer(dataAndChecksumLength);
        if (!isFullPacketInBuffer)
            throw new PackageIsCorruptedException("Received incomplete package.");
        _serialPort.Read(buffer, 2, dataAndChecksumLength); // read to end of packet

        var fullPacketLength = headerLength + dataAndChecksumLength;

        var calculatedCrc = _crc16Calculator.Calculate(buffer, 0, headerLength + dataLength);
        var receivedCrc = buffer[fullPacketLength - 2] | (buffer[fullPacketLength - 1] << 8);
        if (calculatedCrc != receivedCrc)
            throw new PackageIsCorruptedException("Packet checksum is invalid");

        if (buffer[0] != (byte)CommandResponse.Ok)
        {
            byte maxCommandResponseCode = (byte)Enum.GetValues(typeof(CommandResponse)).Cast<CommandResponse>().Max();
            if (buffer[0] <= maxCommandResponseCode)
                throw new CommandResponseException(CommandResponse.WrongCommand);
            else
                throw new UnexpectedResponseException("Unexpected error code received");
        }

        var resultData = new byte[dataLength];
        Array.Copy(buffer, headerLength, resultData, 0, dataLength);
        return resultData;
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