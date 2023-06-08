using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Avalonia.Collections;
using ReactiveUI;
using SolderingStationClient.Models;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class DeviceViewModel : ViewModelBase, IDeviceViewModel
{
    private readonly Device _device;
    private readonly ITemperatureControllerViewModelFactory _temperatureControllerViewModelFactory;

    private string _name = string.Empty;

    public DeviceViewModel(
        ITemperatureControllerViewModelFactory temperatureControllerViewModelFactory, Device device)
    {
        _temperatureControllerViewModelFactory = Guard.Against.Null(temperatureControllerViewModelFactory);
        _device = device;
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public ulong DeviceId { get; set; }

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            foreach (var controller in TemperatureControllers)
                controller.IsActive = value;
            base.IsActive = value;
        }
    }

    public IAvaloniaList<ITemperatureControllerSettingsViewModel> TemperatureControllers { get; } =
        new AvaloniaList<ITemperatureControllerSettingsViewModel>();

    public async Task Init()
    {
        DeviceId = _device.Id;
        Name = $"[{_device.ConnectionName}] {_device.Name}";

        foreach (var controllerKey in _device.TemperatureControllersKeys)
        {
            ITemperatureControllerSettingsViewModel controllerVm;
            if(_device.SupportsPid)
                controllerVm = _temperatureControllerViewModelFactory.CreatePidTemperatureController(controllerKey);
            else
                controllerVm = _temperatureControllerViewModelFactory.CreateTemperatureController(controllerKey);

            await controllerVm.UpdateViewModel();
            TemperatureControllers.Add(controllerVm);
        }
    }
}