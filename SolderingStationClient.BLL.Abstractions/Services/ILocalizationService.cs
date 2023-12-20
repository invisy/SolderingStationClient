using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface ILocalizationService
{
    IEnumerable<Locale> GetAvailableLocalizations();
    string GetCurrentLanguageCode();
    void SaveSelectedLocalization(uint languageId);
}