using ReactiveUI;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class SerialPortInfoViewModel : ViewModelBase
{
    private bool _isConnected;

    public SerialPortInfoViewModel(string serialPortName, bool isConnected)
    {
        SerialPortName = serialPortName;
        _isConnected = isConnected;
    }

    public string SerialPortName { get; }

    public bool IsConnected
    {
        get => _isConnected;
        set => this.RaiseAndSetIfChanged(ref _isConnected, value);
    }
}