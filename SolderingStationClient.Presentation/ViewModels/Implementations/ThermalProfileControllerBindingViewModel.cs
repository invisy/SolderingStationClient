using ReactiveUI;
using SolderingStationClient.Models;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ThermalProfileControllerBindingViewModel : ViewModelBase
{
    private TemperatureControllerViewModel? _selectedController;

    private ThermalProfileRunnerWindowViewModel _parent;
    
    public ControllerThermalProfile ControllerThermalProfile { get; }
    public TemperatureControllerViewModel? SelectedTemperatureController
    {
        get => _selectedController;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedController, value);
            _parent.Update(this, value);
        }
    }

    public ThermalProfileRunnerWindowViewModel Parent => _parent;

    public ThermalProfileControllerBindingViewModel(
        ThermalProfileRunnerWindowViewModel parent, 
        ControllerThermalProfile controllerThermalProfile)
    {
        _parent = parent;
        ControllerThermalProfile = controllerThermalProfile;
    }
    
}