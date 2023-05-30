using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using OxyPlot;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;
using SolderingStationClient.Presentation.ViewModels.Interfaces;
using IResourceProvider = SolderingStationClient.Presentation.Services.IResourceProvider;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class JobPlotViewModel : ViewModelBase, IJobPlotViewModel
{
    private bool _isDisposed;
    
    private readonly IDevicesService _devicesService;
    private readonly IApplicationDispatcher _applicationDispatcher;
    private readonly ITemperatureMonitorService _temperatureMonitorService;
    private readonly IResourceProvider _resourceProvider;

    public PlotModel Model { get; private set; }

    public AvaloniaList<TemperatureControllerJobPlotViewModel> TemperatureControllers { get; } = new();

    public JobPlotViewModel(IApplicationDispatcher applicationDispatcher,
        IResourceProvider resourceProvider, 
        IPlotModelFactory plotModelFactory,
        IDevicesService devicesService,
        ITemperatureMonitorService temperatureMonitorService)
    {
        _applicationDispatcher = applicationDispatcher;
        _devicesService = devicesService;
        _temperatureMonitorService = temperatureMonitorService;
        _resourceProvider = resourceProvider;

        Model = plotModelFactory.Create();
        UpdatePlotTitles();
    }

    public async Task Init(IEnumerable<ThermalProfileControllerBinding> bindings)
    {
        foreach (var binding in bindings)
        {
            var device = await _devicesService.GetDevice(binding.TemperatureControllerKey.DeviceId);
            var name = $"{device.ConnectionName}:{binding.TemperatureControllerKey.ChannelId}";
            var controller = new TemperatureControllerJobPlotViewModel(name, binding.TemperatureControllerKey, binding.ControllerThermalProfile);
            AddTemperatureController(controller);
        }
        
        _devicesService.DeviceDisconnected += OnDisconnectedDevice;
        _temperatureMonitorService.NewTemperatureMeasurement += OnNewTemperatureMeasurement;
        _resourceProvider.ResourcesChanged += OnResourceChanged;
    }
    
    private async void OnResourceChanged()
    {
        var done = false;
        while (!done)
        {
            try
            {
                UpdatePlotTitles();
                done = true;
            }
            catch (Exception)
            {
                await Task.Delay(100);
            }
        }
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

    private void AddTemperatureController(TemperatureControllerJobPlotViewModel vm)
    {
        TemperatureControllers.Add(vm);
        Model.Series.Add(vm.TemperatureCurve);
        Model.Series.Add(vm.ThermalProfile);
        Model.InvalidatePlot(false);
    }
    
    private void RemoveController(TemperatureControllerJobPlotViewModel controller)
    {
        Model.Series.Remove(controller.TemperatureCurve);
        Model.Series.Remove(controller.ThermalProfile);
        TemperatureControllers.Remove(controller);
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
        
        _devicesService.DeviceDisconnected -= OnDisconnectedDevice;
        _temperatureMonitorService.NewTemperatureMeasurement -= OnNewTemperatureMeasurement;

        if (dispose)
            TemperatureControllers.Clear();

        _isDisposed = true;
    }
}