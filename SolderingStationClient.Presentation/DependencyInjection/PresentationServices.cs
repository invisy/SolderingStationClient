using System.Reactive.Concurrency;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Factories.Implementations;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;
using SolderingStationClient.Presentation.ViewModels.Implementations;
using SolderingStationClient.Presentation.ViewModels.Interfaces;
using Splat;

namespace SolderingStationClient.Presentation.DependencyInjection;

public static class PresentationServices
{
    public static void RegisterPresentationServices(this IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.RegisterConstant<IScheduler>(RxApp.MainThreadScheduler);
        services.RegisterLazySingleton<IApplicationDispatcher>(() => new ApplicationDispatcher());
        services.RegisterLazySingleton<IResourceProvider>(() => new ResourceProvider());
        services.RegisterLazySingleton<IMessageBoxService>(() => new MessageBoxService(
            resolver.GetService<IResourceProvider>()
        ));
        services.RegisterConstant<IViewModelCreator>(new ViewModelCreator(resolver));
        
        services.Register<ISerialPortAdvancedSettingsWindowViewModel>(() => new SerialPortAdvancedSettingsWindowViewModel(
            resolver.GetService<ISerialPortsSettingsService>()   
        ));

        services.Register<ILanguageSettingsViewModel>(() => new LanguageSettingsViewModel(
            resolver.GetService<ILocalizationService>()
        ));

        services.Register<IConnectionViewModel>(() => new ConnectionViewModel(
            resolver.GetService<IApplicationDispatcher>(),
            resolver.GetService<IViewModelCreator>(),
            resolver.GetService<ISerialPortsService>(),
            resolver.GetService<IMessageBoxService>()
        ));

        services.RegisterLazySingleton<IPlotModelFactory>(() => new PlotModelFactory());
        
        services.Register<IMainPlotViewModel>(() => new MainPlotViewModel(
            resolver.GetService<IApplicationDispatcher>(),
            resolver.GetService<IResourceProvider>(),
            resolver.GetService<IPlotModelFactory>(),
            resolver.GetService<IDevicesService>(),
            resolver.GetService<ITemperatureMonitorService>()
        ));

        services.Register<ITemperatureControllerViewModelFactory>(() =>
            new DefaultTemperatureControllerViewModelFactory(
                resolver.GetService<ITemperatureControllerService>(),
                resolver.GetService<IPidTemperatureControllerService>()
            ));

        services.Register<IDeviceViewModelFactory>(() => new DefaultDeviceViewModelFactory(
            resolver.GetService<ITemperatureControllerViewModelFactory>()
        ));

        services.Register<IDevicesListViewModel>(() => new DevicesListViewModel(
            resolver.GetService<IApplicationDispatcher>(),
            resolver.GetService<IDeviceViewModelFactory>(),
            resolver.GetService<IDevicesService>(),
            resolver.GetService<ITemperatureMonitorService>()
        ));
        
        services.Register<IThermalProfileEditorWindowViewModel>(() => new ThermalProfileEditorWindowViewModel(
            resolver.GetService<IThermalProfileService>()
        ));
        
        services.Register<IThermalProfileRunnerWindowViewModel>(() => new ThermalProfileRunnerWindowViewModel(
            resolver.GetService<IThermalProfileProcessingService>()
        ));

        services.Register<IMainWindowViewModel>(() => new MainWindowViewModel(
            resolver.GetService<IScheduler>(),
            resolver.GetService<IViewModelCreator>(),
            resolver.GetService<IJobStateService>(),
            resolver.GetService<IThermalProfileProcessingService>()
        ));
    }
}