using SolderingStation.Hardware.Abstractions.Capabilities;
using SolderingStation.Hardware.Abstractions.Connections;
using SolderingStation.Hardware.Models.ConnectionParameters;

namespace SolderingStation.Hardware.Abstractions.Devices.SelfBuiltDeviceV1;

public interface ISelfBuiltDeviceV1<TConnectionParameters, TConnection> : IDevice, IPidControllerCapability
    where TConnectionParameters : IConnectionParameters
    where TConnection : IConnectionGeneric<TConnectionParameters>, ISelfBuiltDeviceCommandProcessor
{
}