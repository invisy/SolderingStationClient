using OxyPlot.Annotations;
using OxyPlot.Series;
using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class TemperatureControllerViewModel : ViewModelBase
{
    public TemperatureControllerKey Key { get; }
    public LineSeries TemperatureCurve { get; }
    public LineAnnotation DesiredTemperature { get; }

    public TemperatureControllerViewModel(TemperatureControllerKey key, LineSeries temperatureCurve, LineAnnotation desiredTemperature)
    {
        Key = key;
        TemperatureCurve = temperatureCurve;
        DesiredTemperature = desiredTemperature;
    }
}