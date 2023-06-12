using System.Collections.Generic;
using System.Threading.Tasks;
using SolderingStationClient.Models;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;

public interface IMainPlotViewModelFactory
{
    Task<IMainPlotViewModel> CreateIdlePlot();
    Task<IMainPlotViewModel> CreateJobPlot(IEnumerable<ThermalProfileControllerBinding> bindings);
}