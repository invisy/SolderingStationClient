using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ThermalProfileRunnerViewModel : IThermalProfileRunnerViewModel
{
    private readonly IThermalProfileProcessingService _thermalProfileProcessingService;
    
    private AvaloniaList<ThermalProfile> ThermalProfilesList { get; } = new();
    private AvaloniaList<TemperatureControllerViewModel> AvailableControllers { get; } = new();

    public ThermalProfileRunnerViewModel(IThermalProfileProcessingService thermalProfileProcessingService)
    {
        _thermalProfileProcessingService = thermalProfileProcessingService;
    }

    public async Task Start()
    {
        var thermalProfiles = (await _thermalProfileProcessingService.GetAllThermalProfiles()).ToList();
        var controllers = thermalProfiles[0].ControllersThermalProfiles.ToList();
        var binding = new List<ThermalProfileControllerBinding>()
        {
            new ThermalProfileControllerBinding(controllers[0], new TemperatureControllerKey(1, 0))
        };
        await _thermalProfileProcessingService.Start(binding);
    }

}