using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface ISerialPortAdvancedSettingsWindowViewModel : IViewModelBase
{
    public Task Edit(string portName);
    public string PortName { get; set; }
    public double BaudRate { get; set; }
    public double DataBits { get; set; }
    public int SelectedParity { get; set; }
    public int SelectedStopBits { get; set; }

    public ReactiveCommand<Unit, Unit> ApplyCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
}