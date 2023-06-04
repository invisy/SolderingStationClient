using System.Collections.Concurrent;
using SolderingStation.Hardware.Abstractions;
using SolderingStation.Hardware.Models;
using SolderingStation.Hardware.Models.ConnectionParameters;
using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.Dto;

namespace SolderingStationClient.BLL.Implementation.Services;

public class SerialPortsService : ISerialPortsService
{
    private readonly IDeviceManager _deviceManager;
    private readonly IHardwareDetector<SerialConnectionParameters> _hardwareDetector;
    private readonly ISerialPortsSettingsService _serialPortsSettingsService;

    private readonly ConcurrentDictionary<string, SerialPortInfo> _serialPorts = new();
    private readonly ISerialPortsMonitor _serialPortsMonitor;

    public SerialPortsService(IDeviceManager deviceManager,
        IHardwareDetector<SerialConnectionParameters> hardwareDetector,
        ISerialPortsMonitor serialPortsMonitor,
        ISerialPortsSettingsService serialPortsSettingsService)
    {
        _deviceManager = deviceManager;
        _hardwareDetector = hardwareDetector;
        _serialPortsMonitor = serialPortsMonitor;
        _serialPortsSettingsService = serialPortsSettingsService;

        SubscribeEvents();
        _serialPortsMonitor.Start(1000);
    }

    public IEnumerable<SerialPortInfoDto> SerialPorts => _serialPorts.Values.Select(info => PortInfoToDto(info));

    public event EventHandler<SerialPortInfoEventArgs>? PortAdded;
    public event EventHandler<SerialPortRemovedEventArgs>? PortRemoved;
    public event EventHandler<SerialPortInfoEventArgs>? PortInfoUpdateEvent;

    public async Task Connect(string portName)
    {
        var serialPortSettings = await _serialPortsSettingsService.GetByPortName(portName) ?? new SerialPortSettings(portName);
        
        var portExists = _serialPorts.TryGetValue(portName, out var port);

        if (!portExists || port?.ConnectedDeviceId != 0)
            return;

        var id = await _hardwareDetector.ConnectDeviceWithIdentification(Map(serialPortSettings));
        UpdatePortInfo(portName, id);
    }

    public void Disconnect(string portName)
    {
        if (!_serialPorts.TryGetValue(portName, out var port))
            return;

        _deviceManager.RemoveDevice(port.ConnectedDeviceId);
        UpdatePortInfo(portName, 0);
    }

    private void SubscribeEvents()
    {
        _serialPortsMonitor.PortPresenceStatusChanged += OnPortPresenceChanged;
    }

    private void UpdatePortInfo(string portName, ulong id)
    {
        if (!_serialPorts.TryGetValue(portName, out var portInfo))
            return;

        portInfo.ConnectedDeviceId = id;
        PortInfoUpdateEvent?.Invoke(this, new SerialPortInfoEventArgs(PortInfoToDto(portInfo)));
    }

    private void OnPortPresenceChanged(object? sender, SerialPortPresenceEventArgs eventArgs)
    {
        switch (eventArgs.Status)
        {
            case PresenceStatus.Connected:
                var portInfo = new SerialPortInfo(eventArgs.PortName, 0);
                _serialPorts.TryAdd(eventArgs.PortName, portInfo);
                PortAdded?.Invoke(this, new SerialPortInfoEventArgs(PortInfoToDto(portInfo)));
                break;
            case PresenceStatus.Disconnected:
                if (_serialPorts.TryGetValue(eventArgs.PortName, out portInfo) && portInfo.ConnectedDeviceId != 0)
                    _deviceManager.RemoveDevice(portInfo.ConnectedDeviceId);
                _serialPorts.Remove(eventArgs.PortName, out _);
                PortRemoved?.Invoke(this, new SerialPortRemovedEventArgs(eventArgs.PortName));
                break;
        }
    }

    private SerialPortInfoDto PortInfoToDto(SerialPortInfo portInfo)
    {
        var isConnected = portInfo.ConnectedDeviceId != 0;
        return new SerialPortInfoDto(portInfo.SerialPortName, isConnected);
    }

    private SerialPortSettings Map(SerialConnectionParameters parameters)
    {
        return new SerialPortSettings(
            parameters.PortName, parameters.BaudRate, parameters.Parity, parameters.DataBits, parameters.StopBits);
    }
    
    private SerialConnectionParameters Map(SerialPortSettings parameters)
    {
        return new SerialConnectionParameters(
            parameters.PortName, parameters.BaudRate, parameters.Parity, parameters.DataBits, parameters.StopBits);
    }
}