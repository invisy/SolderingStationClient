using System.Linq;
using Avalonia.Collections;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class MainPlotViewModel : ViewModelBase, IMainPlotViewModel
{
    public PlotModel Model { get; } = new PlotModel();
    private readonly IResourceProvider _resourceProvider;

    private AvaloniaList<TemperatureControllerViewModel> _temperatureControllers = new();

    public MainPlotViewModel(IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
        
        var vm = new TemperatureControllerViewModel(
            new TemperatureControllerKey(1,2),
            new LineSeries()
            {
                Points = { new DataPoint(1,5), new DataPoint(3,8) }
            },
            new LineAnnotation()
            {
                Type = LineAnnotationType.Horizontal,
                Y = 50
            }
        );

        Init();
    }

    public void Init()
    {
        Model.LegendOrientation = LegendOrientation.Horizontal;
        Model.LegendBorderThickness = 0;
        Model.LegendPlacement = LegendPlacement.Outside;
        Model.LegendPosition = LegendPosition.TopCenter;
        Model.PlotAreaBorderColor = OxyColor.Parse("#999999");
        Model.PlotMargins = new OxyThickness(50, 15, 30, 60);
        Model.Title = _resourceProvider.GetResourceByName<string>("Localization.TemperaturePlotTitle");
        
        Model.Axes.Add(new LinearAxis()
        {
            AbsoluteMinimum = 0,
            AxisTitleDistance = 15,
            MajorStep = 10,
            Minimum = 0,
            MinimumRange = 100,
            MinorStep = 1,
            Position = AxisPosition.Left,
            Title= _resourceProvider.GetResourceByName<string>("Localization.PlotTemperatureLabel"),
            TitleColor = OxyColors.Chocolate,
            TitleFontSize = 12,
            TitleFontWeight = FontWeights.Bold
        });
        
        Model.Axes.Add(new LinearAxis()
        {
            AbsoluteMinimum = 0,
            AxisTitleDistance = 10,
            MajorStep = 10,
            Minimum = 0,
            MinimumRange = 100,
            MinorStep = 1,
            Position = AxisPosition.Bottom,
            Title= _resourceProvider.GetResourceByName<string>("Localization.PlotTimeLabel"),
            TitleColor = OxyColors.Chocolate,
            TitleFontSize = 12,
            TitleFontWeight = FontWeights.Bold
        });
    }

    private void AddTemperatureController(TemperatureControllerViewModel vm)
    {
        _temperatureControllers.Add(vm);
        Model.Series.Add(vm.TemperatureCurve);
        Model.Annotations.Add(vm.DesiredTemperature);
    }
    
    private void RemoveController(TemperatureControllerKey key)
    {
        var controller = _temperatureControllers.First(controller => controller.Key == key);
        var curve = Model.Series.Remove(controller.TemperatureCurve);
        Model.Annotations.Remove(controller.DesiredTemperature);
        _temperatureControllers.Remove(controller);
    }
    
    private void AddTemperature(TemperatureControllerKey key, TemperatureMeasurement measurement)
    {
        var controller = _temperatureControllers.First(controller => controller.Key == key);
        controller.TemperatureCurve.Points.Add(new DataPoint(measurement.SecondsElapsed, measurement.Temperature));
    }
}