namespace SolderingStation.Entities;

public class ProfileEntity : BaseEntity<int>
{
    public string Name { get; }
    public LanguageEntity Language { get; }
    public IEnumerable<ThermalProfileEntity> Parts { get; }
}