using SolderingStationClient.Models;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;
using SolderingStationClient.Presentation.ViewModels.Implementations;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Factories.Implementations;

public class DefaultDeviceViewModelFactory : IDeviceViewModelFactory
{
    private readonly IMessageBoxService _messageBoxService;
    private readonly ITemperatureControllerViewModelFactory _temperatureControllerViewModelFactory;

    public DefaultDeviceViewModelFactory(IMessageBoxService messageBoxService,
        ITemperatureControllerViewModelFactory temperatureControllerViewModelFactory)
    {
        _messageBoxService = messageBoxService;
        _temperatureControllerViewModelFactory = temperatureControllerViewModelFactory;
    }

    public IDeviceViewModel Create(Device device)
    {
        return new DeviceViewModel(_messageBoxService, _temperatureControllerViewModelFactory, device);
    }
}