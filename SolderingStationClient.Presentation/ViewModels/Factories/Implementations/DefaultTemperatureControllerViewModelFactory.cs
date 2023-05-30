using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;
using SolderingStationClient.Presentation.ViewModels.Implementations;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Factories.Implementations;

public class DefaultTemperatureControllerViewModelFactory : ITemperatureControllerViewModelFactory
{
    private readonly IPidTemperatureControllerService _pidTemperatureControllerService;
    private readonly ITemperatureControllerService _temperatureControllerService;

    public DefaultTemperatureControllerViewModelFactory(
        ITemperatureControllerService temperatureControllerService,
        IPidTemperatureControllerService pidTemperatureControllerService)
    {
        _temperatureControllerService = temperatureControllerService;
        _pidTemperatureControllerService = pidTemperatureControllerService;
    }

    public ITemperatureControllerSettingsViewModel CreateTemperatureController(TemperatureControllerKey key)
    {
        return new TemperatureControllerSettingsViewModel(_temperatureControllerService, key);
    }

    public ITemperatureControllerSettingsViewModel CreatePidTemperatureController(TemperatureControllerKey key)
    {
        return new PidTemperatureControllerSettingsViewModel(_pidTemperatureControllerService, key);
    }
}