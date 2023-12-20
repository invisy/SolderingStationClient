namespace SolderingStation.Entities;

public class ThermalProfileEntity : BaseEntity<uint>
{
    public string Name { get; set; }
    public IList<ThermalProfilePartEntity> Parts { get; set; }
    public uint ProfileId { get; set; }

    //For EF
    public ThermalProfileEntity()
    {
    }

    public ThermalProfileEntity(uint id, string name, IEnumerable<ThermalProfilePartEntity> parts, uint profileId)
    {
        Id = id;
        Name = name;
        Parts = new List<ThermalProfilePartEntity>(parts);
        ProfileId = profileId;
    }
}