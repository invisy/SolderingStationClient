﻿using SolderingStation.Hardware.Models;
using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Implementation;

public class SerialPortsMonitor : ISerialPortsMonitor
{
    private readonly ISerialPortsProvider _serialPortsProvider;
    private readonly ITimer _timer;
    private bool _isDisposed;

    private List<string> _lastDetectedPortNames = new();

    public SerialPortsMonitor(ITimer timer, ISerialPortsProvider serialPortsProvider)
    {
        _timer = timer;
        _serialPortsProvider = serialPortsProvider;
        _timer.TimerIntervalElapsed += CheckPortsStatus;
    }

    public IReadOnlyList<string> AllPortNames => _lastDetectedPortNames.AsReadOnly();
    public event EventHandler<SerialPortPresenceEventArgs>? PortPresenceStatusChanged;

    public void Start(uint scanPeriodMs)
    {
        _timer.Interval = scanPeriodMs;
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void CheckPortsStatus(object sender, DateTime dateTime)
    {
        var availablePorts = _serialPortsProvider.GetAvailablePorts().ToList();

        var connectedPorts = availablePorts.Except(_lastDetectedPortNames);
        var disconnectedPorts = _lastDetectedPortNames.Except(availablePorts);

        foreach (var port in connectedPorts)
            PortPresenceStatusChanged?.Invoke(this, new SerialPortPresenceEventArgs(port, PresenceStatus.Connected));

        foreach (var port in disconnectedPorts)
            PortPresenceStatusChanged?.Invoke(this, new SerialPortPresenceEventArgs(port, PresenceStatus.Disconnected));

        _lastDetectedPortNames = availablePorts;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing)
            _timer.Dispose();

        _isDisposed = true;
    }
}