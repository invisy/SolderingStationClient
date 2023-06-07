using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Collections;
using ReactiveUI;
using SolderingStationClient.Presentation.ViewModels.Implementations;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IThermalProfileEditorWindowViewModel : IViewModelBase
{
    Task Init();
    ThermalProfileViewModel? SelectedThermalProfile { get; }
    IAvaloniaList<ThermalProfileViewModel> ThermalProfiles { get; }
    void Create();
    void Remove();
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
}