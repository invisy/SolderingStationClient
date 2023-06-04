using Ardalis.Specification;
using SolderingStation.Entities;

namespace SolderingStationClient.BLL.Implementation.Specifications;

public sealed class ThermalProfileWithAllDataByUserSpecification : Specification<ThermalProfileEntity>
{
    public ThermalProfileWithAllDataByUserSpecification(uint profileId)
    {
        Query
            .Where(entity => entity.ProfileId == profileId)
            .Include(entity => entity.Parts)
            .ThenInclude(partEntity => partEntity.TemperatureCurve);
    }
}