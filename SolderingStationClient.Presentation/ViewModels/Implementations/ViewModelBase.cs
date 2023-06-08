using ReactiveUI;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ViewModelBase : ReactiveObject, IViewModelBase
{
    protected bool _isActive = true;

    public virtual bool IsActive
    {
        get => _isActive;
        set => this.RaiseAndSetIfChanged(ref _isActive, value);
    }
}