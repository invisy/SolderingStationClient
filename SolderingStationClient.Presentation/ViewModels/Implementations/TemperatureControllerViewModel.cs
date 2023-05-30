using Avalonia.Media;
using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class TemperatureControllerViewModel : ViewModelBase
{
    public TemperatureControllerKey Key { get; }
    public Color Color { get; set; }
    public string Name { get; }

    public TemperatureControllerViewModel(string name, TemperatureControllerKey key)
    {
        Name = name;
        Key = key;
    }
}