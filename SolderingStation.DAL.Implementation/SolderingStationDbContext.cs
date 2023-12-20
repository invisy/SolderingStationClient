using LiteDB;
using SolderingStation.Entities;

namespace SolderingStation.DAL.Implementation;

public class SolderingStationDbContext : LiteDatabase
{
    public ILiteCollection<ProfileEntity> Profiles { get; }
    public ILiteCollection<LanguageEntity> Languages { get; }

    public SolderingStationDbContext(string dbPath) : base(dbPath)
    {
        Profiles = GetCollection<ProfileEntity>();
        Languages = GetCollection<LanguageEntity>();
    }
}