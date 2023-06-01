using SolderingStation.Entities;

namespace SolderingStation.DAL.Abstractions;

public interface IUnitOfWork
{
    IGenericRepository<ProfileEntity> ProfilesRepository { get; }
    IGenericRepository<LanguageEntity> LanguagesRepository { get; }
    IGenericRepository<SerialConnectionParametersEntity> SerialConnectionParametersRepository { get; }
    IGenericRepository<ThermalProfileEntity> ThermalProfilesRepository { get; }
    IGenericRepository<ThermalProfilePartEntity> ThermalProfilePartsRepository { get; }
    IGenericRepository<TemperatureMeasurementPointEntity> TemperatureMeasurementPointsRepository { get; }
    
    Task SaveChanges();
    Task BeginTransaction();
    Task TransactionCommit();
    Task TransactionRollback();
}