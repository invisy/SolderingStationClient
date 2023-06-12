using System;
using OxyPlot;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IMainPlotViewModel : IViewModelBase, IDisposable
{
    public PlotModel Model { get; }
}