using OxyPlot;

namespace SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;

public interface IPlotModelFactory
{
    PlotModel Create();
    PlotModel Create(string title, string xAxisTitle, string yAxisTitle);
}