namespace SolderingStation.Entities;

public class ProfileEntity : BaseEntity<int>
{
    public ProfileEntity(string name, int languageId)
    {
        Name = name;
        LanguageId = languageId;
    }

    public string Name { get; set;}
    public int LanguageId { get; set; }
    public LanguageEntity Language { get; set;}
    public IEnumerable<ThermalProfileEntity> ThermalProfiles { get; set;}
    public IEnumerable<SerialConnectionParametersEntity> SerialConnectionsParameters { get; set; }
}