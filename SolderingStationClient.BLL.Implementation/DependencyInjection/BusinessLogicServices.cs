using SolderingStation.Hardware.Abstractions;
using SolderingStation.Hardware.Models.ConnectionParameters;
using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.BLL.Implementation.Services;
using Splat;

namespace SolderingStationClient.BLL.Implementation.DependencyInjection;

public static class BusinessLogicServices
{
    public static void RegisterBusinessLogicServices(this IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.Register<ITimer>(() => new Timer());
        services.RegisterLazySingleton<ISerialPortsProvider>(() => new SerialPortsProvider());
        services.RegisterLazySingleton<ISerialPortsMonitor>(() => new SerialPortsMonitor(
            resolver.GetService<ITimer>(),
            resolver.GetService<ISerialPortsProvider>()
        ));

        services.RegisterLazySingleton<ISerialPortsService>(() => new SerialPortsService(
            resolver.GetService<IDeviceManager>(),
            resolver.GetService<IHardwareDetector<SerialConnectionParameters>>(),
            resolver.GetService<ISerialPortsMonitor>()
        ));
        services.Register<ILocalizationService>(() => new LocalizationService());

        services.Register<IDevicesService>(() => new DevicesService(
            resolver.GetService<IDeviceManager>())
        );

        services.Register<ITemperatureControllerService>(() => new TemperatureControllerService(
            resolver.GetService<IDeviceManager>())
        );

        services.Register<IPidTemperatureControllerService>(() => new PidTemperatureControllerService(
            resolver.GetService<IDeviceManager>())
        );
        
        services.RegisterLazySingleton<ITemperatureHistoryTracker>(() => new TemperatureHistoryTracker());
        
        services.RegisterLazySingleton<ITemperatureMonitorService>(() => new TemperatureMonitorService(
            resolver.GetService<ITemperatureControllerService>(),
            resolver.GetService<ITemperatureHistoryTracker>(),
            resolver.GetService<ITimer>()
        ));
        
        services.RegisterLazySingleton<ITemperatureHistoryTracker>(() => new TemperatureHistoryTracker());
        
        services.Register<IThermalProfileService>(() => new ThermalProfileService());
    }
}