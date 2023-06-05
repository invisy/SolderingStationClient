using System.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Media;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;
using SolderingStationClient.Presentation.ViewModels.Interfaces;
using IResourceProvider = SolderingStationClient.Presentation.Services.IResourceProvider;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class MainPlotViewModel : ViewModelBase, IMainPlotViewModel
{
    public PlotModel Model { get; }
    private readonly IApplicationDispatcher _applicationDispatcher;
    private IDevicesService _devicesService;
    private readonly ITemperatureMonitorService _temperatureMonitorService;
    private readonly IResourceProvider _resourceProvider;

    private bool _isAutoUpdatable = true;

    public bool IsAutoUpdatable
    {
        get => _temperatureMonitorService.IsRunning;
        set
        {
            if (value)
            {
                ClearPlot();
                _temperatureMonitorService.Enable();
            }
            else
                _temperatureMonitorService.Disable();
            
            this.RaiseAndSetIfChanged(ref _isAutoUpdatable, value);
        }
    }
    public AvaloniaList<TemperatureControllerViewModel> TemperatureControllers { get; } = new();

    public MainPlotViewModel(IApplicationDispatcher applicationDispatcher,
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
        
        //TODO move call to Init
        Init();
    }

    public void Init()
    {
        _devicesService.DeviceConnected += OnConnectedDevice;
        _devicesService.DeviceDisconnected += OnDisconnectedDevice;
        _temperatureMonitorService.NewTemperatureMeasurement += OnNewTemperatureMeasurement;
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
    }

    private void OnConnectedDevice(object? sender, DeviceConnectedEventArgs args)
    {
        _applicationDispatcher.Dispatch(() =>
        {
            if (TemperatureControllers.Count == 0)
                IsAutoUpdatable = true;
            
            foreach (var key in args.Device.TemperatureControllersKeys)
            {
                var vm = new TemperatureControllerViewModel($"{args.Device.ConnectionName}:{key.ChannelId}", key);
                AddTemperatureController(vm);
            }
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

    private void AddTemperatureController(TemperatureControllerViewModel vm)
    {
        TemperatureControllers.Add(vm);
        Model.Series.Add(vm.TemperatureCurve);
        Model.Annotations.Add(vm.DesiredTemperature);
        Model.InvalidatePlot(false);
    }
    
    private void RemoveController(TemperatureControllerViewModel controller)
    {
        var curve = Model.Series.Remove(controller.TemperatureCurve);
        Model.Annotations.Remove(controller.DesiredTemperature);
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
            controller.DesiredTemperature.Y = args.DesiredTemperature;
            Model.InvalidatePlot(false);
        });
    }
}