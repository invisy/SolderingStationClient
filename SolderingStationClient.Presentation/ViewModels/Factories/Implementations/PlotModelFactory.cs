using OxyPlot;
using OxyPlot.Axes;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Factories.Implementations;

public class PlotModelFactory : IPlotModelFactory
{

    public PlotModel Create()
    {
        return Create("Plot", "X", "Y");
    }

    public PlotModel Create(string title, string xAxisTitle, string yAxisTitle)
    {
        PlotModel plotModel = new PlotModel();

        plotModel.LegendOrientation = LegendOrientation.Horizontal;
        plotModel.LegendBorderThickness = 0;
        plotModel.LegendPlacement = LegendPlacement.Outside;
        plotModel.LegendPosition = LegendPosition.TopCenter;
        plotModel.PlotAreaBorderColor = OxyColor.Parse("#999999");
        plotModel.PlotMargins = new OxyThickness(50, 15, 30, 60);
        plotModel.Title = title;
        
        plotModel.Axes.Add(new LinearAxis()
        {
            AbsoluteMinimum = 0,
            AxisTitleDistance = 15,
            MajorStep = 10,
            Minimum = 0,
            MinimumRange = 100,
            MinorStep = 1,
            Position = AxisPosition.Left,
            Title = yAxisTitle,
            TitleColor = OxyColors.Chocolate,
            TitleFontSize = 12,
            TitleFontWeight = FontWeights.Bold
        });
        
        plotModel.Axes.Add(new LinearAxis()
        {
            AbsoluteMinimum = 0,
            AxisTitleDistance = 10,
            MajorStep = 10,
            Minimum = 0,
            MinimumRange = 100,
            MinorStep = 1,
            Position = AxisPosition.Bottom,
            Title = xAxisTitle,
            TitleColor = OxyColors.Chocolate,
            TitleFontSize = 12,
            TitleFontWeight = FontWeights.Bold
        });
        

        return plotModel;
    }
}