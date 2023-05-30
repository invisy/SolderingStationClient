using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Collections;
using ReactiveUI;
using SolderingStationClient.Presentation.ViewModels.Implementations;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IConnectionViewModel : IViewModelBase
{
    public IAvaloniaList<SerialPortInfoViewModel> AvailablePorts { get; }
    public SerialPortInfoViewModel? SelectedPort { get; set; }
    Interaction<ISerialPortAdvancedSettingsWindowViewModel, Unit> ShowSerialPortAdvancedSettingsWindow { get; }
    public Task Toggle();
    public Task OpenAdvancedSettings();
}