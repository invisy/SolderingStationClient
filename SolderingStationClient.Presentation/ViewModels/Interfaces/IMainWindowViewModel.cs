using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IMainWindowViewModel : IViewModelBase
{
    public IConnectionViewModel ConnectionViewModel { get; }
    public Interaction<IThermalProfileEditorWindowViewModel, Unit> ShowThermalProfileEditorWindow { get; }
    public Task OpenThermalProfileEditorWindow();
}