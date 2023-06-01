using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface ILocalizationService
{
    Task<IEnumerable<Locale>> GetAvailableLocalizations();
    Task<string> GetCurrentLanguageCode();
    Task SaveSelectedLocalization(int languageId);
}