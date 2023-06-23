using ReactiveUI;
using SolderingStationClient.Models;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ThermalProfileControllerBindingViewModel : ViewModelBase
{
    private TemperatureControllerViewModel? _selectedController;

    private IThermalProfileRunnerWindowViewModel _parent;
    
    public ControllerThermalProfile ControllerThermalProfile { get; }
    public TemperatureControllerViewModel? SelectedTemperatureController
    {
        get => _selectedController;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedController, value);
            
            //Call method only if user selected another controller
            if(value != null)
                _parent.UpdateBindings(this, value);
        }
    }

    public IThermalProfileRunnerWindowViewModel Parent => _parent;

    public ThermalProfileControllerBindingViewModel(
        IThermalProfileRunnerWindowViewModel parent, 
        ControllerThermalProfile controllerThermalProfile)
    {
        _parent = parent;
        ControllerThermalProfile = controllerThermalProfile;
    }
    
}