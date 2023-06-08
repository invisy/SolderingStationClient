using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IMainWindowViewModel : IViewModelBase
{
    public bool IsJobRunning { get; set; }
    IConnectionViewModel ConnectionViewModel { get; }
    Interaction<IThermalProfileRunnerWindowViewModel, Unit> ShowThermalProfileRunnerWindow { get; }
    Interaction<IThermalProfileEditorWindowViewModel, Unit> ShowThermalProfileEditorWindow { get; }
    Task OpenThermalProfileRunnerWindow();
    Task OpenThermalProfileEditorWindow();
}