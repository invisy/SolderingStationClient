using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface IThermalProfileService
{
    Task<IEnumerable<ThermalProfile>> GetAll();
    Task<ThermalProfile> GetById(uint thermalProfileId);
    Task Add(ThermalProfile thermalProfile);
    Task Remove(uint thermalProfileId);
    Task Update(ThermalProfile thermalProfile);
}