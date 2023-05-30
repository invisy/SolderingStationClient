namespace SolderingStation.Entities;

public class ThermalProfileEntity : BaseEntity<int>
{
    public string Name { get; }
    public IEnumerable<ThermalProfilePartEntity> Parts { get; }
}