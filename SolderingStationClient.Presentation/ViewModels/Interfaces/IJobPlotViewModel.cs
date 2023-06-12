using System.Collections.Generic;
using System.Threading.Tasks;
using SolderingStationClient.Models;
using SolderingStationClient.Presentation.ViewModels.Implementations;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IJobPlotViewModel : IMainPlotViewModel
{
    Task Init(IEnumerable<ThermalProfileControllerBinding> bindings);
}