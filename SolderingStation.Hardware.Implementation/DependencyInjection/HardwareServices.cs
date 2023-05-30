using Invisy.SerialCommunication.Factories;
using Invisy.SerialCommunication.Utils;
using SolderingStation.Hardware.Abstractions;
using SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Factories;
using SolderingStation.Hardware.Models.ConnectionParameters;
using Splat;

namespace SolderingStation.Hardware.Implementation.DependencyInjection;

public static class HardwareServices
{
    public static void RegisterHardwareServices(this IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<IStructSerializer>(() => new StructSerializer());
        services.RegisterLazySingleton<ICrc16Calculator>(() => new Crc16Calculator());

        services.RegisterLazySingleton<ISerialCommunicationClientFactory>(() => new SerialCommunicationClientFactory(
            resolver.GetService<IStructSerializer>(),
            resolver.GetService<ICrc16Calculator>()
        ));

        var serialDeviceFactoriesList = new List<IDeviceFactory<SerialConnectionParameters>>
        {
            new SelfBuiltDeviceV1SerialFactory(resolver.GetService<ISerialCommunicationClientFactory>())
        };

        services.RegisterLazySingleton<IDeviceManager>(() => new DeviceManager());
        services.RegisterLazySingleton<IHardwareDetector<SerialConnectionParameters>>(() =>
            new HardwareDetector<SerialConnectionParameters>(
                resolver.GetService<IDeviceManager>(),
                serialDeviceFactoriesList
            )
        );
    }
}