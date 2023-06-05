using Avalonia.Media;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Avalonia;
using ReactiveUI;
using SolderingStationClient.Models.TemperatureControllers;
using LineAnnotation = OxyPlot.Annotations.LineAnnotation;
using LineSeries = OxyPlot.Series.LineSeries;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class TemperatureControllerViewModel : ViewModelBase
{
    public string Name { get; }
    public TemperatureControllerKey Key { get; }
    public LineSeries TemperatureCurve { get; }
    public LineAnnotation DesiredTemperature { get; }
    
    private Color _color;
    public Color Color
    {
        get => _color;
        set
        {
            TemperatureCurve.Color = value.ToOxyColor();
            DesiredTemperature.Color = OxyColor.FromArgb(150, value.R, value.G, value.B);
            DesiredTemperature.TextColor = value.ToOxyColor();
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
            DesiredTemperature.StrokeThickness = value ? 1 : 0;
            TemperatureCurve.PlotModel.InvalidatePlot(false);
            this.RaiseAndSetIfChanged(ref _isVisible, value);
        }
    }
    
    public TemperatureControllerViewModel(string name, TemperatureControllerKey key)
    {
        Name = name;
        Key = key;
        TemperatureCurve = new LineSeries();
        DesiredTemperature = new LineAnnotation() { Text = "Expected temperature", Type = LineAnnotationType.Horizontal, Y = 0 };
        Color = Colors.Green;
    }
}