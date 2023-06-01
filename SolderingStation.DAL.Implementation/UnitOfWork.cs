using Microsoft.EntityFrameworkCore.Storage;
using SolderingStation.DAL.Abstractions;
using SolderingStation.DAL.Abstractions.Exceptions;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private readonly SolderingStationDbContext _context;
    private IDbContextTransaction? _transaction;

    public IGenericRepository<ProfileEntity> ProfilesRepository { get; }
    public IGenericRepository<LanguageEntity> LanguagesRepository { get; }
    public IGenericRepository<SerialConnectionParametersEntity> SerialConnectionParametersRepository { get; }
    public IGenericRepository<ThermalProfileEntity> ThermalProfilesRepository { get; }
    public IGenericRepository<ThermalProfilePartEntity> ThermalProfilePartsRepository { get; }
    public IGenericRepository<TemperatureMeasurementPointEntity> TemperatureMeasurementPointsRepository { get; }
    
    public UnitOfWork(SolderingStationDbContext dbContext, 
        IGenericRepository<ProfileEntity> profilesRepository,
        IGenericRepository<LanguageEntity> languagesRepository,
        IGenericRepository<SerialConnectionParametersEntity> serialConnectionParametersRepository,
        IGenericRepository<ThermalProfileEntity> thermalProfilesRepository,
        IGenericRepository<ThermalProfilePartEntity> thermalProfilePartsRepository,
        IGenericRepository<TemperatureMeasurementPointEntity> temperatureMeasurementPointsRepository)
    {
        _context = dbContext;
        ProfilesRepository = profilesRepository;
        LanguagesRepository = languagesRepository;
        SerialConnectionParametersRepository = serialConnectionParametersRepository;
        ThermalProfilesRepository = thermalProfilesRepository;
        ThermalProfilePartsRepository = thermalProfilePartsRepository;
        TemperatureMeasurementPointsRepository = temperatureMeasurementPointsRepository;
    }

    public async Task SaveChanges()
    {
        try
        {
            int result = await _context.SaveChangesAsync();
        
            if (result == 0)
                throw new DatabaseException($"There were no changes applied to database during saving changes task!");
        }
        catch (Exception e) when (e is not DatabaseException)
        {
            throw new DatabaseException($"Exception while applying changes to database!", e);
        }
    }
    
    public async Task BeginTransaction()
    {
        if(_transaction != null)
            throw new DatabaseException($"There is an uncommitted transaction!");
        
        _transaction = await _context.Database.BeginTransactionAsync();
    }
    
    public async Task TransactionCommit()
    {
        if(_transaction == null)
            throw new DatabaseException($"The transaction can not be committed because it has not been started!");
        
        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
    }
    
    public async Task TransactionRollback()
    {
        if (_transaction == null)
            throw new DatabaseException($"The transaction can not be rolled back because it has not been started!");
            
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }
}