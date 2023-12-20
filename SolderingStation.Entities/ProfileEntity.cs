namespace SolderingStation.Entities;

public class ProfileEntity : BaseEntity<uint>
{
    public string Name { get; set;}
    public uint LanguageId { get; set; }
    public IList<ThermalProfileEntity> ThermalProfiles { get; set;}
    public IList<SerialConnectionParametersEntity> SerialConnectionsParameters { get; set; }
    
    //For EF
    public ProfileEntity()
    {
        
    }
    
    public ProfileEntity(uint id, string name, uint languageId, IEnumerable<ThermalProfileEntity> thermalProfileEntities)
    {
        Id = id;
        Name = name;
        LanguageId = languageId;
        ThermalProfiles = new List<ThermalProfileEntity>(thermalProfileEntities);
    }
}