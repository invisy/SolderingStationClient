namespace SolderingStation.Entities;

public class TemperatureMeasurementPointEntity : BaseEntity<uint>
{
    public float SecondsElapsed { get; set; }
    public ushort Temperature { get; set; }

    //For EF
    public TemperatureMeasurementPointEntity()
    {
        
    }

    public TemperatureMeasurementPointEntity(uint id, float secondsElapsed, ushort temperature)
    {
        Id = id;
        SecondsElapsed = secondsElapsed;
        Temperature = temperature;
    }
}