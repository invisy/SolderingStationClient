namespace SolderingStation.Entities;

public class ThermalProfilePartEntity : BaseEntity<int>
{
    public string Name { get; }
    public IEnumerable<TemperatureMeasurementPointEntity> TemperatureCurve { get; }
}