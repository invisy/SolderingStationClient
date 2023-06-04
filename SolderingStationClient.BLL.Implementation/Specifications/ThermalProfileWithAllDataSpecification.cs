using Ardalis.Specification;
using SolderingStation.Entities;

namespace SolderingStationClient.BLL.Implementation.Specifications;

public sealed class ThermalProfileWithAllDataSpecification : Specification<ThermalProfileEntity>
{
    public ThermalProfileWithAllDataSpecification()
    {
        Query
            .Include(entity => entity.Parts)
            .ThenInclude(partEntity => partEntity.TemperatureCurve);
    }
    
    public ThermalProfileWithAllDataSpecification(uint id)
    {
        Query
            .Where(entity => entity.Id == id)
            .Include(entity => entity.Parts)
            .ThenInclude(partEntity => partEntity.TemperatureCurve);
    }
}