using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Factories;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Implementation.Services;

public class ThermalProfileProcessingService : IThermalProfileProcessingService
{
    private readonly ITemperatureControllerService _temperatureControllerService;
    private readonly IThermalProfileService _thermalProfileService;
    private readonly IThermalProfileProcessingJobFactory _thermalProfileProcessingJobFactory;

    private IJob? _job = null;
    
    public ThermalProfileProcessingService(
        ITemperatureControllerService temperatureControllerService,
        IThermalProfileService thermalProfileService,
        IThermalProfileProcessingJobFactory thermalProfileProcessingJobFactory)
    {
        _temperatureControllerService = temperatureControllerService;
        _thermalProfileProcessingJobFactory = thermalProfileProcessingJobFactory;
        _thermalProfileService = thermalProfileService;
    }

    public async Task<IEnumerable<ThermalProfile>> GetAllThermalProfiles()
    {
        return await _thermalProfileService.GetAll();
    }
    
    public async Task<IEnumerable<ThermalProfile>> Select()
    {
        return await _thermalProfileService.GetAll();
    }
    
    public async Task Start(IEnumerable<ThermalProfileControllerBinding> bindings)
    {
        if (_job != null)
            throw new Exception("You can run only 1 job at the same time!");
        
        _job = _thermalProfileProcessingJobFactory.Create(bindings);
        await _job.RunAsync();
    }
    
    public void Stop()
    {
        if (_job == null)
            throw new Exception("Job is not running!");
        _job.Cancel();
    }
}