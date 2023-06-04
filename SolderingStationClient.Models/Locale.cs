namespace SolderingStationClient.Models;

public class Locale
{
    public Locale(uint id, string nativeName, string englishName, string cultureCode)
    {
        Id = id;
        NativeName = nativeName;
        EnglishName = englishName;
        CultureCode = cultureCode;
    }

    public uint Id { get; set; }
    public string NativeName { get; }
    public string EnglishName { get; }
    public string CultureCode { get; }
}