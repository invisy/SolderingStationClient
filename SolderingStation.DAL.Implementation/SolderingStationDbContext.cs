using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation;

public class SolderingStationDbContext : DbContext
{
    #pragma warning disable CS8618 // Required by Entity Framework to suppress warnings

    private readonly string _dbPath;
    
    public DbSet<ProfileEntity> Profiles { get; set; }
    public DbSet<LanguageEntity> Languages { get; set; }
    public DbSet<ThermalProfileEntity> ThermalProfiles { get; set; }
    public DbSet<ThermalProfilePartEntity> ThermalProfileParts { get; set; }
    public DbSet<TemperatureMeasurementPointEntity> TemperatureMeasurementPoints { get; set; }
    public DbSet<SerialConnectionParametersEntity> SerialConnectionsParameters { get; set; }

    public SolderingStationDbContext(DbContextOptions<SolderingStationDbContext> options) : base(options) {}
    
    public SolderingStationDbContext(string dbPath)
    {
        _dbPath = dbPath;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}