using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Factories;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.BLL.Implementation.Jobs;
using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Implementation.Factories;

public class ThermalProfileProcessingJobFactory : IThermalProfileProcessingJobFactory
{
    private readonly ITemperatureMonitorService _temperatureMonitor;
    private readonly ITemperatureControllerService _temperatureControllerService;
    
    public ThermalProfileProcessingJobFactory(ITemperatureMonitorService temperatureMonitor, 
        ITemperatureControllerService temperatureControllerService)
    {
        _temperatureMonitor = temperatureMonitor;
        _temperatureControllerService = temperatureControllerService;
    }
    
    public IThermalProfileProcessingJob Create(IEnumerable<ThermalProfileControllerBinding> bindings)
    {
        return new ThermalProfileProcessingJob(_temperatureMonitor, _temperatureControllerService, bindings);
    }
}