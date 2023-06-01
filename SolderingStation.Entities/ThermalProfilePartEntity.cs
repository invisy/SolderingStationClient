namespace SolderingStation.Entities;

public class ThermalProfilePartEntity : BaseEntity<int>
{
    public string Name { get; set; }
    public IEnumerable<TemperatureMeasurementPointEntity> TemperatureCurve { get; set; }
}