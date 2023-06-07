using Avalonia.Media;
using OxyPlot;
using OxyPlot.Avalonia;
using ReactiveUI;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;
using LineSeries = OxyPlot.Series.LineSeries;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class TemperatureControllerJobPlotViewModel : TemperatureControllerViewModel
{
    private const byte ThermalProfileAlpha = 70;
    
    public LineSeries TemperatureCurve { get; }
    public LineSeries ThermalProfile { get; }
    
    private Color _color;
    public Color Color
    {
        get => _color;
        set
        {
            TemperatureCurve.Color = value.ToOxyColor();
            ThermalProfile.Color = OxyColor.FromArgb(ThermalProfileAlpha, value.R, value.G, value.B);
            this.RaiseAndSetIfChanged(ref _color, value);
        }
    }
    
    private bool _isVisible = true;
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            TemperatureCurve.IsVisible = value;
            ThermalProfile.IsVisible = value;
            TemperatureCurve.PlotModel.InvalidatePlot(false);
            this.RaiseAndSetIfChanged(ref _isVisible, value);
        }
    }
    
    public TemperatureControllerJobPlotViewModel(string name, TemperatureControllerKey key, ControllerThermalProfile thermalProfile): base(name, key)
    {
        TemperatureCurve = new LineSeries();
        ThermalProfile = new LineSeries();
        AddThermalProfile(thermalProfile);
        TemperatureCurve.Color = OxyColor.FromAColor(255, ThermalProfile.Color);
    }

    private void AddThermalProfile(ControllerThermalProfile thermalProfile)
    {
        foreach (var thermalProfilePoint in thermalProfile.TemperatureMeasurements)
            ThermalProfile.Points.Add(new DataPoint(thermalProfilePoint.SecondsElapsed, thermalProfilePoint.Temperature));

        Color color = Color.FromUInt32((uint)thermalProfile.ArgbColor);
        ThermalProfile.Color = OxyColor.FromArgb(ThermalProfileAlpha, color.R, color.G, color.B);
    }
}