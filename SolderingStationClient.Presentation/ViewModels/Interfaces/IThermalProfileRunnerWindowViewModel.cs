using System.Reactive;
using ReactiveUI;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IThermalProfileRunnerWindowViewModel : IViewModelBase
{
    public ReactiveCommand<Unit, Unit> StartCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
}