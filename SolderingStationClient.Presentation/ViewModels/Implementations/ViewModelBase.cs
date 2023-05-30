using ReactiveUI;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ViewModelBase : ReactiveObject, IViewModelBase
{
    public virtual bool IsActive { get; set; } = true;
}