using Avalonia.Collections;
using OxyPlot;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IMainPlotViewModel : IViewModelBase
{
    public PlotModel Model { get; }
}