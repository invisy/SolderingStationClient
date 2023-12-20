using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Collections;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ThermalProfileRunnerWindowViewModel : ViewModelBase, IThermalProfileRunnerWindowViewModel
{
    private readonly IApplicationDispatcher _applicationDispatcher;
    private readonly IDevicesService _devicesService;
    private readonly IThermalProfileService _thermalProfileService;
    private readonly IThermalProfileProcessingService _thermalProfileProcessingService;
    
    private List<ThermalProfile> ThermalProfilesList { get; } = new();
    private bool _startIsPossible;
    private ThermalProfile? _selectedThermalProfile;
    
    public AvaloniaList<ThermalProfileControllerBindingViewModel> ControllersBindings { get; } = new();
    public List<TemperatureControllerViewModel> AvailableControllers { get; } = new();

    public bool StartIsPossible
    {
        get => _startIsPossible;
        set => this.RaiseAndSetIfChanged(ref _startIsPossible, value);
    }

    public ThermalProfile? SelectedThermalProfile
    {
        get => _selectedThermalProfile;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedThermalProfile, value);
            ReloadBindings(_selectedThermalProfile);
        }
    }
    

    public ThermalProfileRunnerWindowViewModel(
        IApplicationDispatcher applicationDispatcher,
        IDevicesService devicesService,
        IThermalProfileService thermalProfileService,
        IThermalProfileProcessingService thermalProfileProcessingService)
    {
        _applicationDispatcher = applicationDispatcher;
        _thermalProfileProcessingService = thermalProfileProcessingService;
        _thermalProfileService = thermalProfileService;
        _devicesService = devicesService;
        StartCommand = ReactiveCommand.CreateFromTask(Start);
    }

    public async Task Init()
    {
        await _applicationDispatcher.DispatchAsync(async () =>
        {
            var thermalProfiles = _thermalProfileService.GetAll();
            ThermalProfilesList.AddRange(thermalProfiles);

            if (ThermalProfilesList.Count > 0)
                SelectedThermalProfile = ThermalProfilesList.First();


            await ReloadDevicesList();
            _devicesService.DeviceDisconnected += OnDeviceDisconnected;
        });
    }
    
    public void UpdateBindings(ThermalProfileControllerBindingViewModel selectedBinding, TemperatureControllerViewModel? selectedController)
    {
        foreach (var binding in ControllersBindings)
        {
            if (binding == selectedBinding)
                continue;
            
            if (binding.SelectedTemperatureController == selectedController)
                binding.SelectedTemperatureController = null;
        }

        foreach (var binding in ControllersBindings)
        {
            if (binding.SelectedTemperatureController == null)
            {
                StartIsPossible = false;
                return;
            }
        }
        
        StartIsPossible = true;
    }

    private void ReloadBindings(ThermalProfile? thermalProfile)
    {
        StartIsPossible = false;
        ControllersBindings.Clear();
        if (thermalProfile == null)
            return;
        
        foreach (var controllerThermalProfile in thermalProfile.ControllersThermalProfiles)
        {
            var vm = new ThermalProfileControllerBindingViewModel(this, controllerThermalProfile);
            ControllersBindings.Add(vm);
        }
    }

    private void OnDeviceDisconnected(object? sender, DeviceDisconnectedEventArgs args)
    {
        _applicationDispatcher.Dispatch(() =>
        {
            var searchedControllers = AvailableControllers.Where(deviceVm => deviceVm.Key.DeviceId == args.DeviceId).ToList();
            foreach (var controller in searchedControllers)
                AvailableControllers.Remove(controller);
            ReloadBindings(_selectedThermalProfile);
        });
    }
    
    private async Task ReloadDevicesList()
    {
        await _applicationDispatcher.DispatchAsync(async () =>
        {
            AvailableControllers.Clear();
            var devices = await _devicesService.GetDevices();
            foreach (var device in devices)
                AddDevice(device);
        });
    }
    
    private void AddDevice(Device device)
    {
        foreach (var key in device.TemperatureControllersKeys)
        {
            var vm = new TemperatureControllerViewModel($"{device.ConnectionName}:{key.ChannelId}", key);
            AvailableControllers.Add(vm);
        }
    }

    public ReactiveCommand<Unit, IEnumerable<ThermalProfileControllerBinding>> StartCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; } = ReactiveCommand.Create(() => Unit.Default);

    public async Task<IEnumerable<ThermalProfileControllerBinding>> Start()
    {
        var bindings = new List<ThermalProfileControllerBinding>();
        foreach (var controllerBinding in ControllersBindings)
        {
            bindings.Add(new ThermalProfileControllerBinding(controllerBinding.ControllerThermalProfile, 
                controllerBinding.SelectedTemperatureController!.Key));
        }
        
        await Task.Factory.StartNew(() => _thermalProfileProcessingService.Start(bindings));

        return bindings;
    }
}