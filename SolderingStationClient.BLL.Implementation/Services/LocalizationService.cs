using SolderingStation.DAL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.BLL.Implementation.Specifications;
using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Implementation.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserProfileService _userProfileService;

    public LocalizationService(IUnitOfWork uow, IUserProfileService userProfileService)
    {
        _uow = uow;
        _userProfileService = userProfileService;
    }

    public async Task<IEnumerable<Locale>> GetAvailableLocalizations()
    {
        var localizations = await _uow.LanguagesRepository.GetListAsync();

        var locales = localizations.Select(localization =>
            new Locale(localization.Id, localization.NativeName, localization.EnglishName, localization.Code));
        
        return locales;
    }

    public async Task<string> GetCurrentLanguageCode()
    {
        var currentProfileId = _userProfileService.GetProfileId();
        var spec = new ProfileWithLanguageSpecification(currentProfileId);
        var profile = await _uow.ProfilesRepository.GetBySpecAsync(spec);
        if (profile is null)
            throw new ArgumentException("Profile doesn`t exist");
        
        return profile.Language.Code;
    }

    public async Task SaveSelectedLocalization(uint languageId)
    {
        var currentProfileId = _userProfileService.GetProfileId();
        var profile = await _uow.ProfilesRepository.GetByIdAsync(currentProfileId);
        if (profile is null)
            throw new ArgumentException("Profile doesn`t exist");
        profile.LanguageId = languageId;
        _uow.ProfilesRepository.Update(profile);
        await _uow.SaveChanges();
    }
}