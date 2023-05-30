using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface IThermalProfileProcessingService
{
    Task<IEnumerable<ThermalProfile>> GetAllThermalProfiles();
    Task Start(IEnumerable<ThermalProfileControllerBinding> bindings);
    public void Stop();
}