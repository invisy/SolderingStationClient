using Ardalis.Specification;
using SolderingStation.Entities;

namespace SolderingStationClient.BLL.Implementation.Specifications;

public sealed class ProfileWithLanguageSpecification : Specification<ProfileEntity>
{
    public ProfileWithLanguageSpecification(uint id)
    {
        Query
            .Where(entity => entity.Id == id)
            .Include(entity => entity.Language);
    }
}