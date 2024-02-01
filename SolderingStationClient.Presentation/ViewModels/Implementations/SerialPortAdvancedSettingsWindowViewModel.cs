using System.IO.Ports;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.Models;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class SerialPortAdvancedSettingsWindowViewModel : ViewModelBase, ISerialPortAdvancedSettingsWindowViewModel
{
    private readonly ISerialPortsSettingsService _serialPortsSettingsService;
    
    private double _baudRate;
    private double _dataBits;
    private int _parity;
    private int _stopBits;
    

    private string _portName = string.Empty;

    public SerialPortAdvancedSettingsWindowViewModel(ISerialPortsSettingsService serialPortsSettingsService)
    {
        _serialPortsSettingsService = serialPortsSettingsService;
        ApplyCommand = ReactiveCommand.Create(Apply);
    }

    public void Init(string portName)
    {
        var settings = _serialPortsSettingsService.GetByPortName(portName);
        if (settings == null)
        {
            settings = new SerialPortSettings(portName);
            _serialPortsSettingsService.Add(settings);
        }
        
        PortName = portName;
        BaudRate = settings.BaudRate;
        DataBits = settings.DataBits;
        SelectedParity = (int)settings.Parity;
        SelectedStopBits = (int)settings.StopBits;
    }
    
    public string PortName
    {
        get => _portName;
        set => this.RaiseAndSetIfChanged(ref _portName, value);
    }

    public double BaudRate
    {
        get => _baudRate;
        set => this.RaiseAndSetIfChanged(ref _baudRate, value);
    }

    public double DataBits
    {
        get => _dataBits;
        set => this.RaiseAndSetIfChanged(ref _dataBits, value);
    }
    
    public int SelectedParity
    {
        get => _parity;
        set => this.RaiseAndSetIfChanged(ref _parity, value);
    }

    public int SelectedStopBits
    {
        get => _stopBits;
        set => this.RaiseAndSetIfChanged(ref _stopBits, value);
    }

    private void Apply()
    {
        var portSettings = new SerialPortSettings(PortName, (int)BaudRate, (Parity)SelectedParity, (int)DataBits,
            (StopBits)SelectedStopBits);
        _serialPortsSettingsService.Remove(portSettings.PortName);
        _serialPortsSettingsService.Add(portSettings);
    }
    
    public ReactiveCommand<Unit, Unit> ApplyCommand { get; }

    public ReactiveCommand<Unit, Unit> CloseCommand { get; } = ReactiveCommand.Create(() => Unit.Default);
}