using SolderingStation.DAL.Implementation;
using SolderingStation.Entities;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Implementation.Services;

public class LocalizationService : ILocalizationService
{
    private readonly SolderingStationDbContext _context;
    private readonly IUserProfileService _userProfileService;

    public LocalizationService(SolderingStationDbContext context, IUserProfileService userProfileService)
    {
        _context = context;
        _userProfileService = userProfileService;
    }

    public IEnumerable<Locale> GetAvailableLocalizations()
    {
        var localizations = _context.GetCollection<LanguageEntity>().FindAll();

        var locales = localizations.Select(localization =>
            new Locale(localization.Id, localization.NativeName, localization.EnglishName, localization.Code));
        
        return locales;
    }

    public string GetCurrentLanguageCode()
    {
        var currentProfileId = _userProfileService.GetProfileId();
        var profiles = _context.GetCollection<ProfileEntity>();
        var profile = profiles.FindOne(p => p.Id == currentProfileId);
       
        if (profile is null)
            throw new ArgumentException("Profile doesn`t exist");
        
        var languages = _context.GetCollection<LanguageEntity>().FindOne(l => l.Id == profile.LanguageId);
        
        return languages.Code;
    }

    public void SaveSelectedLocalization(uint languageId)
    {
        var currentProfileId = _userProfileService.GetProfileId();
        var profilesCollection = _context.GetCollection<ProfileEntity>();
        var profile = profilesCollection.FindOne(profiles => profiles.Id == currentProfileId);
        if (profile is null)
            throw new ArgumentException("Profile doesn`t exist");
        profile.LanguageId = languageId;
        profilesCollection.Update(profile);
    }
}