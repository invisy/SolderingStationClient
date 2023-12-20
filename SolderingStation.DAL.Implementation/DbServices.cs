using Splat;

namespace SolderingStation.DAL.Implementation;

public static class DbServices
{
    public static void RegisterDbServices(this IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.RegisterConstant<SolderingStationDbContext>(
            new SolderingStationDbContext("SolderingStationLite.db")
        );
    }
}