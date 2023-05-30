using System.Reactive;
using Avalonia.Collections;
using ReactiveUI;
using SolderingStationClient.Presentation.ViewModels.Implementations;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IThermalProfileEditorWindowViewModel
{
    ThermalProfileViewModel? SelectedThermalProfile { get; }
    IAvaloniaList<ThermalProfileViewModel> ThermalProfiles { get; }
    void Create();
    void Remove();
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
}