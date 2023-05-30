using System.Threading.Tasks;
using Avalonia.Collections;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IDeviceViewModel : IViewModelBase
{
    ulong DeviceId { get; }
    public string Name { get; }
    public IAvaloniaList<ITemperatureControllerSettingsViewModel> TemperatureControllers { get; }
    Task Init();
}