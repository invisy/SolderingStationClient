using Avalonia.Media;
using OxyPlot;
using OxyPlot.Avalonia;
using ReactiveUI;
using SolderingStationClient.Models;
using LineSeries = OxyPlot.Series.LineSeries;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ThermalProfilePartViewModel: ViewModelBase
{
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    
    public LineSeries Curve { get; } = new LineSeries();

    private Color _color;
    public Color Color
    {
        get => _color;
        set
        {
            this.RaiseAndSetIfChanged(ref _color, value);
            Curve.Color = _color.ToOxyColor();
            Curve.PlotModel.InvalidatePlot(false);
        }
    }

    public ThermalProfilePartViewModel(ControllerThermalProfile thermalProfilePart)
    {
        Name = thermalProfilePart.Name;

        foreach (var point in thermalProfilePart.TemperatureMeasurements)
            Curve.Points.Add(new DataPoint(point.SecondsElapsed, point.Temperature));
        _color = Color.FromUInt32((uint)thermalProfilePart.ArgbColor);
        Curve.Color = _color.ToOxyColor();
    }
    
    public ThermalProfilePartViewModel(string name)
    {
        Name = name;
        Curve.Points.Add(new DataPoint(0, 0));
        _color = Colors.Green;
        Curve.Color = _color.ToOxyColor();
    }
}