using Invisy.SerialCommunication.Models;

namespace Invisy.SerialCommunication.Factories;

public interface ISerialCommunicationClientFactory
{
    ISerialCommunicationClient Create(SerialPortSettings portSettings);
    ISerialCommunicationClient Create(SerialPortSettings portSettings, int responseTimeout);
}