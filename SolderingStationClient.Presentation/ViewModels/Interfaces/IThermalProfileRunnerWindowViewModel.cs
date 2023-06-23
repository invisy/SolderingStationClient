using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Collections;
using ReactiveUI;
using SolderingStationClient.Models;
using SolderingStationClient.Presentation.ViewModels.Implementations;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IThermalProfileRunnerWindowViewModel : IViewModelBase
{
    AvaloniaList<ThermalProfileControllerBindingViewModel> ControllersBindings { get; }
    List<TemperatureControllerViewModel> AvailableControllers { get; }
    bool StartIsPossible { get; }
    
    ReactiveCommand<Unit, IEnumerable<ThermalProfileControllerBinding>> StartCommand { get; }
    ReactiveCommand<Unit, Unit> CloseCommand { get; }

    Task Init();
    void UpdateBindings(ThermalProfileControllerBindingViewModel selectedBinding,
        TemperatureControllerViewModel? selectedController);
}