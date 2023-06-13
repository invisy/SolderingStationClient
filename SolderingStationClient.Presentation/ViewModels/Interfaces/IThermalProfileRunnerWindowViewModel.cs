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
    Task Init();
    bool StartIsPossible { get; }
    AvaloniaList<ThermalProfile> ThermalProfilesList { get; }
    AvaloniaList<ThermalProfileControllerBindingViewModel> ControllersBindings { get; }
    ReactiveCommand<Unit, IEnumerable<ThermalProfileControllerBinding>> StartCommand { get; }
    ReactiveCommand<Unit, Unit> CloseCommand { get; }
}