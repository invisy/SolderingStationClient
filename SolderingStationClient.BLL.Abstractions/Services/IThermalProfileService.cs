using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface IThermalProfileService
{
    IEnumerable<ThermalProfile> GetAll();
    ThermalProfile GetById(uint thermalProfileId);
    void Add(ThermalProfile thermalProfile);
    void Remove(uint thermalProfileId);
    void Update(ThermalProfile thermalProfile);
}