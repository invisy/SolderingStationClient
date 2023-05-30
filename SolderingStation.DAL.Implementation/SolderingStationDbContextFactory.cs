using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SolderingStation.DAL.Implementation;


public class SolderingStationDbContextFactory : IDesignTimeDbContextFactory<SolderingStationDbContext>
{
    public SolderingStationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SolderingStationDbContext>();
        optionsBuilder.UseSqlite("Data Source=test.db");

        return new SolderingStationDbContext(optionsBuilder.Options);
    }
}