using Invisy.SerialCommunicationProtocol.Factories;
using Invisy.SerialCommunicationProtocol.Models;
using SolderingStation.Hardware.Abstractions;
using SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Connections;
using SolderingStation.Hardware.Models.ConnectionParameters;

namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Factories;

public class SelfBuiltDeviceV1SerialFactory : IDeviceFactory<SerialConnectionParameters>
{
    private readonly ISerialCommunicationClientFactory _serialCommunicationClientFactory;

    public SelfBuiltDeviceV1SerialFactory(ISerialCommunicationClientFactory serialCommunicationClientFactory)
    {
        _serialCommunicationClientFactory = serialCommunicationClientFactory;
    }

    public IDevice Create(SerialConnectionParameters parameters)
    {
        var serialPortSettings = new SerialPortSettings(parameters.PortName, parameters.BaudRate, parameters.Parity,
            parameters.DataBits, parameters.StopBits);

        var client = _serialCommunicationClientFactory.Create(serialPortSettings);
        var connection = new SelfBuiltDeviceSerialConnection(client, parameters);

        return new SelfBuiltDeviceV1<SerialConnectionParameters, SelfBuiltDeviceSerialConnection>(connection);
    }
}