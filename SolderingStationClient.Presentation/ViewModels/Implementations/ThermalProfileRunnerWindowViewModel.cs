using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Collections;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ThermalProfileRunnerWindowViewModel : ViewModelBase, IThermalProfileRunnerWindowViewModel
{
    private readonly IThermalProfileProcessingService _thermalProfileProcessingService;
    
    private AvaloniaList<ThermalProfile> ThermalProfilesList { get; } = new();
    private AvaloniaList<TemperatureControllerViewModel> AvailableControllers { get; } = new();

    public ThermalProfileRunnerWindowViewModel(IThermalProfileProcessingService thermalProfileProcessingService)
    {
        _thermalProfileProcessingService = thermalProfileProcessingService;
        StartCommand = ReactiveCommand.CreateFromTask(Start);
    }
    
    public ReactiveCommand<Unit, Unit> StartCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; } = ReactiveCommand.Create(() => Unit.Default);

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