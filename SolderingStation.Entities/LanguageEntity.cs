namespace SolderingStation.Entities;

public class LanguageEntity : BaseEntity<uint>
{
    //For EF
    public LanguageEntity()
    {
        
    }
    
    public LanguageEntity(uint id, string englishName, string nativeName, string code)
    {
        Id = id;
        EnglishName = englishName;
        NativeName = nativeName;
        Code = code;
    }

    public string EnglishName { get; set; }
    public string NativeName { get; set;}
    public string Code { get; set;}
}