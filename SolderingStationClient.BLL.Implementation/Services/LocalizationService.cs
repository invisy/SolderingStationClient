using System.Globalization;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Implementation.Services;

public class LocalizationService : ILocalizationService
{
    public string GetCurrentLanguageCode()
    {
        //if database is clean

        return "en";
    }

    public IEnumerable<Locale> GetLanguagesByCodesList(IList<string> languageCodes)
    {
        var languages = new List<Locale>();
        var cultures = GetAvailableCultures(languageCodes);

        foreach (var culture in cultures)
            languages.Add(new Locale(culture.NativeName, culture.EnglishName, culture.TwoLetterISOLanguageName));

        return languages;
    }

    private IEnumerable<CultureInfo> GetAvailableCultures(IList<string> languageCodes)
    {
        var result = new List<CultureInfo>();

        foreach (var languageCode in languageCodes)
            try
            {
                result.Add(CultureInfo.GetCultureInfoByIetfLanguageTag(languageCode));
            }
            catch (Exception e)
            {
                //Log info
                //Console.WriteLine(e);
            }

        return result;
    }
}