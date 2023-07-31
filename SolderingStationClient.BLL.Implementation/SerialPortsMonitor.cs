using SolderingStation.Hardware.Models;
using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Implementation;

public class SerialPortsMonitor : ISerialPortsMonitor
{
    private readonly ISerialPortsProvider _serialPortsProvider;
    private readonly ITimer _timer;

    private List<string> _lastDetectedPortNames = new();

    public SerialPortsMonitor(ITimer timer, ISerialPortsProvider serialPortsProvider)
    {
        _timer = timer;
        _serialPortsProvider = serialPortsProvider;
    }

    public IReadOnlyList<string> AllPortNames => _lastDetectedPortNames.AsReadOnly();
    public event EventHandler<SerialPortPresenceEventArgs>? PortPresenceStatusChanged;

    public void Start(uint scanPeriodMs)
    {
        _timer.TimerIntervalElapsed += CheckPortsStatus;
        _timer.Interval = scanPeriodMs;
        _timer.Start();
    }

    public void Stop()
    {
        _timer.TimerIntervalElapsed -= CheckPortsStatus;
        _timer.Stop();
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
}