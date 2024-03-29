﻿using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Abstractions;

public interface ISerialPortsMonitor
{
    IReadOnlyList<string> AllPortNames { get; }
    event EventHandler<SerialPortPresenceEventArgs> PortPresenceStatusChanged;
    void Start(uint scanPeriodMs);
    void Stop();
}