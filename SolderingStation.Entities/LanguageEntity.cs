namespace SolderingStation.Entities;

public class LanguageEntity : BaseEntity<int>
{
    public LanguageEntity(string englishName, string nativeName, string code)
    {
        EnglishName = englishName;
        NativeName = nativeName;
        Code = code;
    }

    public string EnglishName { get; set; }
    public string NativeName { get; set;}
    public string Code { get; set;}
}