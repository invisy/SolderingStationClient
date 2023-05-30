using Invisy.SerialCommunication.Models;
using Invisy.SerialCommunication.Utils;

namespace Invisy.SerialCommunication.Factories;

public class SerialCommunicationClientFactory : ISerialCommunicationClientFactory
{
    private readonly ICrc16Calculator _crc16Calculator;
    private readonly IStructSerializer _structSerializer;

    public SerialCommunicationClientFactory(IStructSerializer structSerializer,
        ICrc16Calculator crc16Calculator)
    {
        _structSerializer = structSerializer;
        _crc16Calculator = crc16Calculator;
    }

    public ISerialCommunicationClient Create(SerialPortSettings portSettings)
    {
        var serialPort = new SerialPortDefault();
        return new SerialCommunicationClient(serialPort, _structSerializer, _crc16Calculator, portSettings);
    }

    public ISerialCommunicationClient Create(SerialPortSettings portSettings, int responseTimeout)
    {
        var serialPort = new SerialPortDefault();
        return new SerialCommunicationClient(serialPort, _structSerializer, _crc16Calculator, portSettings,
            responseTimeout);
    }
}