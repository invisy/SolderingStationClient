using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;
using SolderingStationClient.Presentation.ViewModels.Implementations;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Factories.Implementations;

public class DefaultTemperatureControllerViewModelFactory : ITemperatureControllerViewModelFactory
{
    private readonly IMessageBoxService _messageBoxService;
    private readonly IPidTemperatureControllerService _pidTemperatureControllerService;
    private readonly ITemperatureControllerService _temperatureControllerService;

    public DefaultTemperatureControllerViewModelFactory(
        IMessageBoxService messageBoxService,
        ITemperatureControllerService temperatureControllerService,
        IPidTemperatureControllerService pidTemperatureControllerService)
    {
        _messageBoxService = messageBoxService;
        _temperatureControllerService = temperatureControllerService;
        _pidTemperatureControllerService = pidTemperatureControllerService;
    }

    public ITemperatureControllerSettingsViewModel CreateTemperatureController(TemperatureControllerKey key)
    {
        return new TemperatureControllerSettingsViewModel(_messageBoxService, _temperatureControllerService, key);
    }

    public ITemperatureControllerSettingsViewModel CreatePidTemperatureController(TemperatureControllerKey key)
    {
        return new PidTemperatureControllerSettingsViewModel(_messageBoxService, _pidTemperatureControllerService, key);
    }
}