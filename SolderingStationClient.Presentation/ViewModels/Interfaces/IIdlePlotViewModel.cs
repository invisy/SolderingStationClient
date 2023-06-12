using System.Threading.Tasks;
using SolderingStationClient.Presentation.ViewModels.Implementations;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IIdlePlotViewModel : IMainPlotViewModel
{
    Task Init();
}