using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SolderingStationClient.Models;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IThermalProfileRunnerWindowViewModel : IViewModelBase
{
    public ReactiveCommand<Unit, IEnumerable<ThermalProfileControllerBinding>> StartCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
}