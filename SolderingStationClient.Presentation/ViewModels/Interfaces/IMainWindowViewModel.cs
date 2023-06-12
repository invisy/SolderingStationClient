using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using SolderingStationClient.Models;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IMainWindowViewModel : IViewModelBase
{
    public bool IsJobRunning { get; set; }
    IConnectionViewModel ConnectionViewModel { get; }
    IMainPlotViewModel MainPlotViewModel { get; }
    Interaction<IThermalProfileEditorWindowViewModel, Unit> ShowThermalProfileEditorWindow { get; }
    Interaction<IThermalProfileRunnerWindowViewModel, IEnumerable<ThermalProfileControllerBinding>?> ShowThermalProfileRunnerWindow { get; }
    Task OpenThermalProfileRunnerWindow();
    Task OpenThermalProfileEditorWindow();
}