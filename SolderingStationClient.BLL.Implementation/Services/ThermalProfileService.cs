using System.Drawing;
using SolderingStation.DAL.Abstractions;
using SolderingStation.Entities;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.BLL.Implementation.Specifications;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Implementation.Services;

public class ThermalProfileService : IThermalProfileService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserProfileService _userProfileService;

    public ThermalProfileService(IUnitOfWork uow, IUserProfileService userProfileService)
    {
        _uow = uow;
        _userProfileService = userProfileService;
    }

    public async Task<IEnumerable<ThermalProfile>> GetAll()
    {
        var spec = new ThermalProfileWithAllDataSpecification();
        var result = await _uow.ThermalProfilesRepository.GetListBySpecAsync(spec);
        return result.Select(Map);
    }

    public async Task<ThermalProfile> GetById(uint thermalProfileId)
    {
        var spec = new ThermalProfileWithAllDataSpecification(thermalProfileId);
        var result = await _uow.ThermalProfilesRepository.GetBySpecAsync(spec);
        if (result == null)
            throw new ArgumentNullException(nameof(thermalProfileId));
        
        return Map(result);
    }

    public async Task Add(ThermalProfile thermalProfile)
    {
        var userProfileId = _userProfileService.GetProfileId();
        var thermalProfileEntity = Map(thermalProfile);
        thermalProfileEntity.ProfileId = userProfileId;
        _uow.ThermalProfilesRepository.Add(thermalProfileEntity);
        await _uow.SaveChanges();
    }

    public async Task Remove(uint thermalProfileId)
    {
        _uow.ThermalProfilesRepository.Delete(thermalProfileId);
        await _uow.SaveChanges();
    }

    public async Task Update(ThermalProfile thermalProfile)
    {
        _uow.ThermalProfilesRepository.Update(Map(thermalProfile));
        await _uow.SaveChanges();
    }

    
    private ThermalProfile Map(ThermalProfileEntity entity)
    {
        var controllerThermalProfiles = entity.Parts.Select(Map).ToList();
        return new ThermalProfile(entity.Id, entity.Name, controllerThermalProfiles);
    }
    
    private ThermalProfileEntity Map(ThermalProfile model)
    {
        var controllerThermalProfilesParts = model.ControllersThermalProfiles.Select(Map).ToList();
        return new ThermalProfileEntity(model.Id, model.Name, controllerThermalProfilesParts);
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
    
    private TemperatureMeasurementPoint Map(TemperatureMeasurementPointEntity entity)
    {
        return new TemperatureMeasurementPoint(entity.Id, entity.SecondsElapsed, entity.Temperature);
    }
    
    private TemperatureMeasurementPointEntity Map(TemperatureMeasurementPoint model)
    {
        return new TemperatureMeasurementPointEntity(model.Id, model.SecondsElapsed, model.Temperature);
    }
}