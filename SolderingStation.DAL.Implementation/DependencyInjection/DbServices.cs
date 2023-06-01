using SolderingStation.DAL.Abstractions;
using SolderingStation.Entities;
using Splat;

namespace SolderingStation.DAL.Implementation.DependencyInjection;

public static class DbServices
{
    public static void RegisterDbServices(this IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.RegisterConstant<SolderingStationDbContext>(
            new SolderingStationDbContext("SolderingStationDatabase.db")
        );
        
        services.RegisterConstant<IGenericRepository<ProfileEntity>>(
            new GenericRepository<ProfileEntity>(resolver.GetService<SolderingStationDbContext>())
        );
        services.RegisterConstant<IGenericRepository<LanguageEntity>>(
            new GenericRepository<LanguageEntity>(resolver.GetService<SolderingStationDbContext>())
        );
        services.RegisterConstant<IGenericRepository<SerialConnectionParametersEntity>>(
            new GenericRepository<SerialConnectionParametersEntity>(resolver.GetService<SolderingStationDbContext>())
        );
        services.RegisterConstant<IGenericRepository<ThermalProfileEntity>>(
            new GenericRepository<ThermalProfileEntity>(resolver.GetService<SolderingStationDbContext>())
        );
        services.RegisterConstant<IGenericRepository<ThermalProfilePartEntity>>(
            new GenericRepository<ThermalProfilePartEntity>(resolver.GetService<SolderingStationDbContext>())
        );
        services.RegisterConstant<IGenericRepository<TemperatureMeasurementPointEntity>>(
            new GenericRepository<TemperatureMeasurementPointEntity>(resolver.GetService<SolderingStationDbContext>())
        );
        
        services.RegisterConstant<IUnitOfWork>(
            new UnitOfWork(
                resolver.GetService<SolderingStationDbContext>(),
                resolver.GetService<IGenericRepository<ProfileEntity>>(),
                resolver.GetService<IGenericRepository<LanguageEntity>>(),
                resolver.GetService<IGenericRepository<SerialConnectionParametersEntity>>(),
                resolver.GetService<IGenericRepository<ThermalProfileEntity>>(),
                resolver.GetService<IGenericRepository<ThermalProfilePartEntity>>(),
                resolver.GetService<IGenericRepository<TemperatureMeasurementPointEntity>>()
            )
        );
    }
}