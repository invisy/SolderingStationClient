using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IMainWindowViewModel : IViewModelBase
{
    IConnectionViewModel ConnectionViewModel { get; }
    Interaction<IThermalProfileRunnerViewModel, Unit> ShowThermalProfileSelectorWindow { get; }
    Interaction<IThermalProfileEditorWindowViewModel, Unit> ShowThermalProfileEditorWindow { get; }
    Task OpenThermalProfileEditorWindow();
}