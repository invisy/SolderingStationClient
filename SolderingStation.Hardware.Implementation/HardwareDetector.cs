using SolderingStation.Hardware.Abstractions;
using SolderingStation.Hardware.Abstractions.Exceptions;
using SolderingStation.Hardware.Models.ConnectionParameters;

namespace SolderingStation.Hardware.Implementation;

public class HardwareDetector<T> : IHardwareDetector<T> where T : IConnectionParameters
{
    private readonly IDeviceManager _deviceManager;
    private readonly IEnumerable<IDeviceFactory<T>> _supportedDeviceFactories;

    public HardwareDetector(IDeviceManager deviceManager, IEnumerable<IDeviceFactory<T>> supportedDeviceFactories)
    {
        _deviceManager = deviceManager;
        _supportedDeviceFactories = supportedDeviceFactories;
    }

    public async Task<ulong> ConnectDeviceWithIdentification(T parameters)
    {
        foreach (var deviceFactory in _supportedDeviceFactories)
        {
            var device = deviceFactory.Create(parameters);
            var isSuccessfullyConnected = device.Connect() && await device.Probe();

            if (isSuccessfullyConnected)
            {
                var id = _deviceManager.AddDevice(device);
                return id;
            }

            device.Dispose();
        }

        throw new UnsupportedDeviceException();
    }
}