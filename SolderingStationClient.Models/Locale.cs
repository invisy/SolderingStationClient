namespace SolderingStationClient.Models;

public class Locale
{
    public Locale(string nativeName, string englishName, string cultureCode)
    {
        NativeName = nativeName;
        EnglishName = englishName;
        CultureCode = cultureCode;
    }

    public string NativeName { get; set; }
    public string EnglishName { get; set; }
    public string CultureCode { get; set; }
}