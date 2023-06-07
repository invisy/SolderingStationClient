using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IMainWindowViewModel : IViewModelBase
{
    IConnectionViewModel ConnectionViewModel { get; }
    Interaction<IThermalProfileRunnerWindowViewModel, Unit> ShowThermalProfileSelectorWindow { get; }
    Interaction<IThermalProfileEditorWindowViewModel, Unit> ShowThermalProfileEditorWindow { get; }
    Task OpenThermalProfileRunnerWindow();
    Task OpenThermalProfileEditorWindow();
}