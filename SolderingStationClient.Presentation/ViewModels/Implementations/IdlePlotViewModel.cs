using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using OxyPlot;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;
using SolderingStationClient.Presentation.ViewModels.Interfaces;
using IResourceProvider = SolderingStationClient.Presentation.Services.IResourceProvider;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class IdlePlotViewModel : ViewModelBase, IIdlePlotViewModel
{
    private bool _isDisposed;
    private const int UpdateInterval = 1000;
    
    private readonly IPlotModelFactory _plotModelFactory;
    private IDevicesService _devicesService;
    private readonly IApplicationDispatcher _applicationDispatcher;
    private readonly ITemperatureMonitorService _temperatureMonitorService;
    private readonly IResourceProvider _resourceProvider;

    private bool _isAutoUpdatable = true;

    public PlotModel Model { get; private set; }
    public bool IsAutoUpdatable
    {
        get => _isAutoUpdatable;
        set
        {
            if (value)
            {
                ClearPlot();
                _temperatureMonitorService.Enable(UpdateInterval);
            }
            else
                _temperatureMonitorService.Disable();
            
            this.RaiseAndSetIfChanged(ref _isAutoUpdatable, _temperatureMonitorService.IsRunning);
        }
    }
    public AvaloniaList<TemperatureControllerIdlePlotViewModel> TemperatureControllers { get; } = new();

    public IdlePlotViewModel(IApplicationDispatcher applicationDispatcher,
        IResourceProvider resourceProvider, 
        IPlotModelFactory plotModelFactory,
        IDevicesService devicesService,
        ITemperatureMonitorService temperatureMonitorService)
    {
        _applicationDispatcher = applicationDispatcher;
        _devicesService = devicesService;
        _temperatureMonitorService = temperatureMonitorService;
        _resourceProvider = resourceProvider;
        _plotModelFactory = plotModelFactory;
    }

    public async Task Init()
    {
        Model = _plotModelFactory.Create();
        UpdatePlotTitles();

        var devices = await _devicesService.GetDevices();
        
        _temperatureMonitorService.NewTemperatureMeasurement += OnNewTemperatureMeasurement;
        
        _applicationDispatcher.Dispatch(() =>
        {
            foreach (var device in devices)
                AddDevice(device);
            
            IsAutoUpdatable = true;
        });
        
        _devicesService.DeviceConnected += OnConnectedDevice;
        _devicesService.DeviceDisconnected += OnDisconnectedDevice;
    }

    public void ClearPlot()
    {
        _temperatureMonitorService.ClearHistory();
        foreach (var controller in TemperatureControllers)
        {
            controller.TemperatureCurve.Points.Clear();
            controller.DesiredTemperature.Y = 0;
        }
        Model.InvalidatePlot(false);
        GC.Collect(2, GCCollectionMode.Forced);
    }

    private void OnConnectedDevice(object? sender, DeviceConnectedEventArgs args)
    {
        _applicationDispatcher.Dispatch(() =>
        {
            ClearPlot();
            AddDevice(args.Device);
        });
    }
    
    private void OnDisconnectedDevice(object? sender, DeviceDisconnectedEventArgs args)
    {
        _applicationDispatcher.Dispatch(() =>
        {
            var controllers = TemperatureControllers.Where(controller => controller.Key.DeviceId == args.DeviceId).ToList();
            foreach (var controller in controllers)
                RemoveController(controller);
        });
    }

    private void UpdatePlotTitles()
    {
        string plotTitle = _resourceProvider.GetResourceByName<string>("Localization.TemperaturePlotTitle");
        string plotXTitle = _resourceProvider.GetResourceByName<string>("Localization.PlotTimeLabel");
        string plotYTitle = _resourceProvider.GetResourceByName<string>("Localization.PlotTemperatureLabel");
        Model.Title = plotTitle;
        foreach (var axis in Model.Axes)
        {
            if (axis.IsHorizontal())
                axis.Title = plotXTitle;
            else
                axis.Title = plotYTitle;
        }
    }

    private void AddDevice(Device device)
    {
        foreach (var key in device.TemperatureControllersKeys)
        {
            var vm = new TemperatureControllerIdlePlotViewModel($"{device.ConnectionName}:{key.ChannelId}", key);
            AddTemperatureController(vm);
        }
    }

    private void AddTemperatureController(TemperatureControllerIdlePlotViewModel vm)
    {
        TemperatureControllers.Add(vm);
        Model.Series.Add(vm.TemperatureCurve);
        Model.Annotations.Add(vm.DesiredTemperature);
        Model.InvalidatePlot(false);
    }
    
    private void RemoveController(TemperatureControllerIdlePlotViewModel controllerIdle)
    {
        Model.Series.Remove(controllerIdle.TemperatureCurve);
        Model.Annotations.Remove(controllerIdle.DesiredTemperature);
        TemperatureControllers.Remove(controllerIdle);
        Model.InvalidatePlot(false);
    }

    private void OnNewTemperatureMeasurement(object? sender, TemperatureMeasurementEventArgs args)
    {
        _applicationDispatcher.Dispatch(() =>
        {
            var controller =
                TemperatureControllers.FirstOrDefault(controller => controller.Key == args.TemperatureControllerId);

            //Controller not found?
            if (controller == null)
                return;

            controller.TemperatureCurve.Points.Add(new DataPoint(args.Temperature.SecondsElapsed,
                args.Temperature.Temperature));
            controller.DesiredTemperature.Y = args.DesiredTemperature;
            Model.InvalidatePlot(false);
        });
    }

    public void Dispose()
    {
        Dispose(true);
    }
    
    protected void Dispose(bool dispose)
    {
        if (_isDisposed)
            return;
        
        _devicesService.DeviceConnected -= OnConnectedDevice;
        _devicesService.DeviceDisconnected -= OnDisconnectedDevice;
        _temperatureMonitorService.NewTemperatureMeasurement -= OnNewTemperatureMeasurement;

        if (dispose)
            TemperatureControllers.Clear();

        _isDisposed = true;
    }
}