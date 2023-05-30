using Ardalis.Specification;
using SolderingStation.Entities;

namespace SolderingStationClient.BLL.Implementation.Specifications;

public sealed class SerialConnectionParametersByPortNameSpecification : Specification<SerialConnectionParametersEntity>
{
    public SerialConnectionParametersByPortNameSpecification(uint userProfileId, string portName)
    {
        Query
            .Where(entity => entity.ProfileId == userProfileId && entity.PortName == portName);
    }
}