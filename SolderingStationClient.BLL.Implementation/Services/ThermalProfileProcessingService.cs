using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Factories;
using SolderingStationClient.BLL.Abstractions.Services;

namespace SolderingStationClient.BLL.Implementation.Services;

public class ThermalProfileProcessingService
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

    public async Task Start()
    {
        _job = _thermalProfileProcessingJobFactory.Create();
        await _job.RunAsync();
    }
    
    public void Stop()
    {
        if (_job == null)
            throw new Exception("Job is not running!");
        _job.Cancel();
    }
    
}