using System.Reactive;
using ReactiveUI;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface ISerialPortAdvancedSettingsWindowViewModel : IViewModelBase
{
    public string PortName { get; }
    public int BaudRate { get; }
    public int DataBits { get; }

    public ReactiveCommand<Unit, Unit> ApplyCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
}