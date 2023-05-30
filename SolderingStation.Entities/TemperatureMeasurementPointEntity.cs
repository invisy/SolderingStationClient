namespace SolderingStation.Entities;

public class TemperatureMeasurementPointEntity : BaseEntity<int>
{
    public float SecondsElapsed { get; }
    public ushort Temperature { get; }
}