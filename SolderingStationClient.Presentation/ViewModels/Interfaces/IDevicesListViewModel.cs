using System.Threading.Tasks;
using Avalonia.Collections;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IDevicesListViewModel : IViewModelBase
{
    Task Init();
    IAvaloniaList<IDeviceViewModel> DevicesList { get; }
}