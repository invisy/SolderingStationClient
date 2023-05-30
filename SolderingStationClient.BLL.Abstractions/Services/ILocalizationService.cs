using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface ILocalizationService
{
    IEnumerable<Locale> GetLanguagesByCodesList(IList<string> languageCodes);
    string GetCurrentLanguageCode();
}