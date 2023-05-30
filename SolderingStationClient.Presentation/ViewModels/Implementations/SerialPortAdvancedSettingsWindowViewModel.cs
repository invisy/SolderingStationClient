using System.Reactive;
using ReactiveUI;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class SerialPortAdvancedSettingsWindowViewModel : ViewModelBase, ISerialPortAdvancedSettingsWindowViewModel
{
    private int _baudRate = 9600;

    private int _dataBits = 8;

    private string _portName = string.Empty;

    public string PortName
    {
        get => _portName;
        set => this.RaiseAndSetIfChanged(ref _portName, value);
    }

    public int BaudRate
    {
        get => _baudRate;
        set => this.RaiseAndSetIfChanged(ref _baudRate, value);
    }

    public int DataBits
    {
        get => _dataBits;
        set => this.RaiseAndSetIfChanged(ref _dataBits, value);
    }

    public ReactiveCommand<Unit, Unit> ApplyCommand { get; } = ReactiveCommand.Create(() =>
    {
        //TODO
        return Unit.Default;
    });

    public ReactiveCommand<Unit, Unit> CloseCommand { get; } = ReactiveCommand.Create(() => Unit.Default);
}