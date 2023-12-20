namespace SolderingStation.Entities;

public class ThermalProfilePartEntity : BaseEntity<uint>
{
    public string Name { get; set; }
    public int Color { get; set; }
    public IList<TemperatureMeasurementPointEntity> TemperatureCurve { get; set; }

    //For EF
    public ThermalProfilePartEntity()
    {
    }

    public ThermalProfilePartEntity(uint id, string name, int color, IEnumerable<TemperatureMeasurementPointEntity> measurements)
    {
        Id = id;
        Name = name;
        Color = color;
        TemperatureCurve = new List<TemperatureMeasurementPointEntity>(measurements);
    }
}