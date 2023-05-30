using SolderingStationClient.Models;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;
using SolderingStationClient.Presentation.ViewModels.Implementations;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Factories.Implementations;

public class DefaultDeviceViewModelFactory : IDeviceViewModelFactory
{
    private readonly ITemperatureControllerViewModelFactory _temperatureControllerViewModelFactory;

    public DefaultDeviceViewModelFactory(ITemperatureControllerViewModelFactory temperatureControllerViewModelFactory)
    {
        _temperatureControllerViewModelFactory = temperatureControllerViewModelFactory;
    }

    public IDeviceViewModel Create(Device device)
    {
        return new DeviceViewModel(_temperatureControllerViewModelFactory, device);
    }
}