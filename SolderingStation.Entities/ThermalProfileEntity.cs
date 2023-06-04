namespace SolderingStation.Entities;

public class ThermalProfileEntity : BaseEntity<uint>
{
    public uint ProfileId { get; set; }
    public string Name { get; set; }
    public IEnumerable<ThermalProfilePartEntity> Parts { get; set; }

    //For EF
    public ThermalProfileEntity()
    {
    }

    public ThermalProfileEntity(uint id, string name, IEnumerable<ThermalProfilePartEntity> parts)
    {
        Id = id;
        Name = name;
        Parts = parts;
    }
}