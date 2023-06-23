using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using OxyPlot;
using ReactiveUI;
using SolderingStationClient.Models;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ThermalProfileViewModel : ViewModelBase
{
    private readonly IResourceProvider _resourceProvider;
    
    private string _name;
    private ThermalProfilePartViewModel? _activeThermalProfilePartViewModel;
    
    public PlotModel PlotModel { get; } = new PlotModel();
    public PlotController PlotController { get; } = new PlotController();
    
    public uint Id { get; }
    
    public string Name { 
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public IAvaloniaList<ThermalProfilePartViewModel> ThermalProfileParts { get; } =
        new AvaloniaList<ThermalProfilePartViewModel>();
    
    public ThermalProfilePartViewModel? ActiveThermalProfilePart
    {
        get => _activeThermalProfilePartViewModel!;
        set => this.RaiseAndSetIfChanged(ref _activeThermalProfilePartViewModel, value);
    }

    public ThermalProfileViewModel(ThermalProfile thermalProfile, IPlotModelFactory plotModelFactory, 
        IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
        
        PlotModel = plotModelFactory.Create();
        UpdatePlotTitles();
        EnableController();
        
        Id = thermalProfile.Id;
        _name = thermalProfile.Name;
        foreach (var thermalProfilePart in thermalProfile.ControllersThermalProfiles)
        {
            var thermalProfileVm = new ThermalProfilePartViewModel(thermalProfilePart);
            ThermalProfileParts.Add(thermalProfileVm);
            PlotModel.Series.Add(thermalProfileVm.Curve);
        }
        
        if(ThermalProfileParts.Count != 0)
            ActiveThermalProfilePart = ThermalProfileParts[0];
        
    }
    
    public ThermalProfileViewModel(string name, IPlotModelFactory plotModelFactory, IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
        
        Id = 0;
        _name = name;

        PlotModel = plotModelFactory.Create();
        UpdatePlotTitles();
        EnableController();
    }

    public void EnableController()
    {
        var command = new DelegatePlotCommand<OxyMouseDownEventArgs>(
            (_, _, a) =>
            {
                a.Handled = true;
                if (ActiveThermalProfilePart == null)
                    return;
                var lastPoint = ActiveThermalProfilePart.Curve.Points.Last();
                var point = ActiveThermalProfilePart.Curve.InverseTransform(a.Position);
                if (lastPoint.X + 1 <= point.X)
                {
                    var dataPoint = new DataPoint(Math.Round(point.X), Math.Round(point.Y));
                    ActiveThermalProfilePart.Curve.Points.Add(dataPoint);
                }
                else
                {
                    var closePoint = GetClosePoint(ActiveThermalProfilePart.Curve.Points, point);
                    if(closePoint != null && closePoint.Value is not { X: 0, Y: 0 })
                        ActiveThermalProfilePart.Curve.Points.Remove(closePoint.Value);
                    else
                        TryInsertPoint(ActiveThermalProfilePart.Curve.Points, point);
                }
                PlotModel.InvalidatePlot(true);
            });
        
        PlotController.BindMouseDown(OxyMouseButton.Left, command);
        PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
    }
    
    private void TryInsertPoint(List<DataPoint> points, DataPoint click)
    {
        for (int i=0;i<points.Count-1;i++)
        {
            var previousPoint = points[i].X;
            var nextPoint = points[i+1].X;
            var distance = nextPoint - previousPoint;
            
            if (previousPoint < click.X && click.X < nextPoint && distance >=2)
            {
                var dataPoint = new DataPoint(Math.Round(click.X), Math.Round(click.Y));
                if (Math.Abs(dataPoint.X - previousPoint) > 0 && Math.Abs(dataPoint.X - nextPoint) > 0)
                {
                    ActiveThermalProfilePart?.Curve.Points.Insert(i+1, dataPoint);
                    return;
                }
            }
        }
    }

    private DataPoint? GetClosePoint(List<DataPoint> points, DataPoint click)
    {
        foreach (var point in points)
        {
            if (Math.Abs(point.X - click.X) < 0.5 && Math.Abs(point.Y - click.Y) < 0.5)
                return point;
        }

        return null;
    }
    
    private void UpdatePlotTitles()
    {
        string plotTitle = _resourceProvider.GetResourceByName<string>("Localization.TemperaturePlotTitle");
        string plotXTitle = _resourceProvider.GetResourceByName<string>("Localization.PlotTimeLabel");
        string plotYTitle = _resourceProvider.GetResourceByName<string>("Localization.PlotTemperatureLabel");

        PlotModel.Title = plotTitle;
        foreach (var axis in PlotModel.Axes)
        {
            axis.Title = axis.IsHorizontal() ? plotXTitle : plotYTitle;
        }
        
        PlotModel.InvalidatePlot(false);
    }

    public void Add()
    {
        var thermalProfileVm = new ThermalProfilePartViewModel("New controller profile");
        ThermalProfileParts.Add(thermalProfileVm);
        PlotModel.Series.Add(thermalProfileVm.Curve);
        PlotModel.InvalidatePlot(true);
    }

    public void Remove()
    {
        if (ActiveThermalProfilePart == null)
            return;
        
        PlotModel.Series.Remove(ActiveThermalProfilePart.Curve);
        ThermalProfileParts.Remove(ActiveThermalProfilePart);
        ActiveThermalProfilePart = ThermalProfileParts.LastOrDefault();
        PlotModel.InvalidatePlot(true);
    }
}