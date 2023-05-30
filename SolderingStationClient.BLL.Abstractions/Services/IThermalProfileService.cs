using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface IThermalProfileService
{
    IEnumerable<ThermalProfile> GetAll();
    ThermalProfile GetById(int thermalProfileId);
    void Add(ThermalProfile thermalProfile);
    void Remove(int thermalProfileId);
    void Update(ThermalProfile thermalProfile);
}