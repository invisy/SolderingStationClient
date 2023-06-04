namespace SolderingStation.Entities;

public class ProfileEntity : BaseEntity<uint>
{
    //For EF
    public ProfileEntity()
    {
        
    }
    
    public ProfileEntity(string name, uint languageId)
    {
        Name = name;
        LanguageId = languageId;
    }

    public string Name { get; set;}
    public uint LanguageId { get; set; }
    public LanguageEntity Language { get; set;}
    public IEnumerable<ThermalProfileEntity> ThermalProfiles { get; set;}
    public IEnumerable<SerialConnectionParametersEntity> SerialConnectionsParameters { get; set; }
}