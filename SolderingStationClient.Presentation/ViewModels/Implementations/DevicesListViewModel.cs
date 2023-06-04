using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Avalonia.Collections;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class DevicesListViewModel : ViewModelBase, IDevicesListViewModel
{
    private readonly IApplicationDispatcher _applicationDispatcher;
    private readonly ITemperatureMonitorService _temperatureMonitor;
    private readonly IDevicesService _devicesService;
    private readonly IDeviceViewModelFactory _deviceViewModelFactory;

    public DevicesListViewModel(
        IApplicationDispatcher applicationDispatcher,
        IDeviceViewModelFactory deviceViewModelFactory, 
        IDevicesService devicesService,
        ITemperatureMonitorService temperatureMonitor)
    {
        _applicationDispatcher = applicationDispatcher;
        _temperatureMonitor = Guard.Against.Null(temperatureMonitor);
        _deviceViewModelFactory = Guard.Against.Null(deviceViewModelFactory);
        _devicesService = Guard.Against.Null(devicesService);
    }

    public IAvaloniaList<IDeviceViewModel> DevicesList { get; } = new AvaloniaList<IDeviceViewModel>();

    public async Task Init()
    {
        var devices = await _devicesService.GetDevices();
        DevicesList.AddRange(devices.Select(device => _deviceViewModelFactory.Create(device)));
        
        _devicesService.DeviceConnected += OnDeviceConnected;
        _devicesService.DeviceDisconnected += OnDeviceDisconnected;
        _temperatureMonitor.NewTemperatureMeasurement += OnTemperatureMeasurement;
        _temperatureMonitor.Enable();
    }

    private async void OnDeviceConnected(object? sender, DeviceConnectedEventArgs args)
    {
        var deviceVm = _deviceViewModelFactory.Create(args.Device);
        await deviceVm.Init();
        
        _applicationDispatcher.Dispatch(() => DevicesList.Add(deviceVm));
    }

    private void OnDeviceDisconnected(object? sender, DeviceDisconnectedEventArgs args)
    {
        _applicationDispatcher.Dispatch(() =>
        {
            var device = DevicesList.FirstOrDefault(deviceVm => deviceVm.DeviceId == args.DeviceId);
            if (device != null)
            {
                DevicesList.Remove(device);
            }
        });
    }
    
    private void OnTemperatureMeasurement(object? sender, TemperatureMeasurementEventArgs args)
    {
        _applicationDispatcher.Dispatch(() =>
        {
            var device = DevicesList.FirstOrDefault(device => device.DeviceId == args.TemperatureControllerId.DeviceId);

            var controller = device?.TemperatureControllers.FirstOrDefault(controller => controller.ControllerId == args.TemperatureControllerId.ChannelId);
            
            //The device is already disconnected. The OnDeviceDisconnected method was called for this class before it was called for the monitor.
            if (controller == null)
                return;
            
            controller.CurrentTemperature = args.Temperature.Temperature;
            controller.DesiredTemperature = args.DesiredTemperature;
        });
    }
}