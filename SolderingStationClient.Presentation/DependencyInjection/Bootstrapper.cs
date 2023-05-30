using SolderingStation.DAL.Implementation.DependencyInjection;
using SolderingStation.Hardware.Implementation.DependencyInjection;
using SolderingStationClient.BLL.Implementation.DependencyInjection;
using Splat;

namespace SolderingStationClient.Presentation.DependencyInjection;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterDbServices(resolver);
        services.RegisterHardwareServices(resolver);
        services.RegisterBusinessLogicServices(resolver);
        services.RegisterPresentationServices(resolver);
    }
}