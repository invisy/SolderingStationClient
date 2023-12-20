using SolderingStation.DAL.Implementation;
using SolderingStation.Entities;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Implementation.Services;

public class ThermalProfileService : IThermalProfileService
{
    private readonly SolderingStationDbContext _context;
    private readonly IUserProfileService _userProfileService;

    public ThermalProfileService(SolderingStationDbContext context, IUserProfileService userProfileService)
    {
        _context = context;
        _userProfileService = userProfileService;
    }

    public IEnumerable<ThermalProfile> GetAll()
    {
        var userProfileId = _userProfileService.GetProfileId();
        var profiles = _context.GetCollection<ProfileEntity>().FindOne(profile => profile.Id == userProfileId);
        
        var result = profiles.ThermalProfiles;
        return result.Select(Map).ToList();
    }

    public ThermalProfile GetById(uint thermalProfileId)
    {
        var userProfileId = _userProfileService.GetProfileId();
        var profiles = _context.GetCollection<ProfileEntity>().FindOne(profile => profile.Id == userProfileId);
        
        var result = profiles.ThermalProfiles.First(thermalProfile => thermalProfile.Id == thermalProfileId);

        if (result == null)
            throw new ArgumentNullException(nameof(thermalProfileId));
        
        return Map(result);
    }

    public void Add(ThermalProfile newThermalProfile)
    {
        var userProfileId = _userProfileService.GetProfileId();
        var profiles = _context.GetCollection<ProfileEntity>();
        var profile = profiles.FindOne(profile => profile.Id == userProfileId);
        
        profile.ThermalProfiles.Add(Map(newThermalProfile));
        
        profiles.Update(profile);
    }

    public void Remove(uint thermalProfileId)
    {
        var userProfileId = _userProfileService.GetProfileId();
        var profiles = _context.GetCollection<ProfileEntity>();
        var profile = profiles.FindOne(profile => profile.Id == userProfileId);
        
        var thermalProfile = profile.ThermalProfiles.First(tp => tp.Id == thermalProfileId);
        profile.ThermalProfiles.Remove(thermalProfile);
        
        profiles.Update(profile);
    }

    public void Update(ThermalProfile newThermalProfile)
    {
        var userProfileId = _userProfileService.GetProfileId();
        var profiles = _context.GetCollection<ProfileEntity>();
        var profile = profiles.FindOne(profile => profile.Id == userProfileId);

        var thermalProfile = profile.ThermalProfiles.First(tp => tp.Id == newThermalProfile.Id);
        profile.ThermalProfiles.Remove(thermalProfile);
        profile.ThermalProfiles.Add(Map(newThermalProfile));
        
        profiles.Update(profile);
    }

    private ThermalProfile Map(ThermalProfileEntity entity)
    {
        var controllerThermalProfiles = entity.Parts.Select(Map).ToList();
        return new ThermalProfile(entity.Id, entity.Name, controllerThermalProfiles);
    }
    
    private ThermalProfileEntity Map(ThermalProfile model)
    {
        var profileId = _userProfileService.GetProfileId();
        var controllerThermalProfilesParts = model.ControllersThermalProfiles.Select(Map).ToList();
        return new ThermalProfileEntity(model.Id, model.Name, controllerThermalProfilesParts, profileId);
    }
    
    private ControllerThermalProfile Map(ThermalProfilePartEntity entity)
    {
        var curve = entity.TemperatureCurve.Select(Map).ToList();
        return new ControllerThermalProfile(entity.Id, entity.Name, entity.Color, curve);
    }
    
    private ThermalProfilePartEntity Map(ControllerThermalProfile model)
    {
        var curve = model.TemperatureMeasurements.Select(Map).ToList();
        return new ThermalProfilePartEntity(model.Id, model.Name, model.ArgbColor, curve);
    }
    
    private ThermalProfilePoint Map(TemperatureMeasurementPointEntity entity)
    {
        return new ThermalProfilePoint(entity.Id, entity.SecondsElapsed, entity.Temperature);
    }
    
    private TemperatureMeasurementPointEntity Map(ThermalProfilePoint model)
    {
        return new TemperatureMeasurementPointEntity(model.Id, model.SecondsElapsed, model.Temperature);
    }
}