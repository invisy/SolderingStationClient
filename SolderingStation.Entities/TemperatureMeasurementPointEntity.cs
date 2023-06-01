namespace SolderingStation.Entities;

public class TemperatureMeasurementPointEntity : BaseEntity<int>
{
    public float SecondsElapsed { get; set; }
    public ushort Temperature { get; set; }
}