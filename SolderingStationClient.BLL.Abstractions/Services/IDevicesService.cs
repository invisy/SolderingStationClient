﻿using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface IDevicesService : IDisposable
{
    event EventHandler<DeviceConnectedEventArgs>? DeviceConnected;
    event EventHandler<DeviceDisconnectedEventArgs>? DeviceDisconnected;
    Task<Device> GetDevice(ulong deviceId);
    Task<IEnumerable<Device>> GetDevices();
}