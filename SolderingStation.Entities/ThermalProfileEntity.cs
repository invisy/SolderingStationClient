namespace SolderingStation.Entities;

public class ThermalProfileEntity : BaseEntity<int>
{
    public string Name { get; set; }
    public IEnumerable<ThermalProfilePartEntity> Parts { get; set; }
}