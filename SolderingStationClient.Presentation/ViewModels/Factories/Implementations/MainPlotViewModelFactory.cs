using System.Collections.Generic;
using System.Threading.Tasks;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;
using SolderingStationClient.Presentation.ViewModels.Implementations;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Factories.Implementations;

public class MainPlotViewModelFactory : IMainPlotViewModelFactory
{
    private readonly IApplicationDispatcher _applicationDispatcher;
    private readonly IResourceProvider _resourceProvider;
    private readonly IPlotModelFactory _plotModelFactory;
    private readonly IDevicesService _devicesService;
    private readonly ITemperatureMonitorService _temperatureMonitorService;
    
    public MainPlotViewModelFactory(
        IApplicationDispatcher applicationDispatcher,
        IResourceProvider resourceProvider, 
        IPlotModelFactory plotModelFactory,
        IDevicesService devicesService,
        ITemperatureMonitorService temperatureMonitorService)
    {
        _applicationDispatcher = applicationDispatcher;
        _resourceProvider = resourceProvider;
        _plotModelFactory = plotModelFactory;
        _devicesService = devicesService;
        _temperatureMonitorService = temperatureMonitorService;

    }

    public async Task<IMainPlotViewModel> CreateIdlePlot()
    {
        var idlePlotViewModel = new IdlePlotViewModel(_applicationDispatcher, _resourceProvider, _plotModelFactory, _devicesService,
            _temperatureMonitorService);
        await idlePlotViewModel.Init();
        
        return idlePlotViewModel;
    }

    public async Task<IMainPlotViewModel> CreateJobPlot(IEnumerable<ThermalProfileControllerBinding> bindings)
    {
        var jobPlotViewModel = new JobPlotViewModel(_applicationDispatcher, _resourceProvider, _plotModelFactory, _devicesService, _temperatureMonitorService);
        await jobPlotViewModel.Init(bindings);
        return jobPlotViewModel;
    }
}