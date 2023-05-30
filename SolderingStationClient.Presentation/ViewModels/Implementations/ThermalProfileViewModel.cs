using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using OxyPlot;
using OxyPlot.Axes;
using ReactiveUI;
using SolderingStationClient.Models;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ThermalProfileViewModel : ViewModelBase
{
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
    
    public ThermalProfilePartViewModel ActiveThermalProfilePart
    {
        get => _activeThermalProfilePartViewModel!;
        set => this.RaiseAndSetIfChanged(ref _activeThermalProfilePartViewModel, value);
    }

    public ThermalProfileViewModel(ThermalProfile thermalProfile)
    {
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
        
        ConfigurePlotModel();
        EnableController();
    }
    
    public ThermalProfileViewModel(string name)
    {
        Id = 0;
        _name = name;

        ConfigurePlotModel();
        EnableController();
    }
    
    public void EnableController()
    {
        var command = new DelegatePlotCommand<OxyMouseDownEventArgs>(
            (v, c, a) =>
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
                    if(closePoint != null)
                        ActiveThermalProfilePart.Curve.Points.Remove(closePoint.Value);
                    else
                    {
                        TryInsertPoint(ActiveThermalProfilePart.Curve.Points, point);
                        var dataPoint = new DataPoint(Math.Round(point.X), Math.Round(point.Y));
                        ActiveThermalProfilePart.Curve.Points.Add(dataPoint);
                    }
                }
                PlotModel.InvalidatePlot(true);
            });
        
        PlotController.BindMouseDown(OxyMouseButton.Left, command);
        PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
    }
    
    private bool TryInsertPoint(List<DataPoint> points, DataPoint click)
    {
        for (int i=0;i<points.Count-1;i++)
        {
            var previousPoint = points[i].X;
            var nextPoint = points[i+1].X;
            var distance = nextPoint - previousPoint;
            
            if (previousPoint < click.X && click.X < nextPoint && distance >=2)
                return true;
        }

        return false;
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

    private void ConfigurePlotModel()
    {
        PlotModel.LegendBorderThickness = 0;
        PlotModel.LegendOrientation = LegendOrientation.Horizontal;
        PlotModel.LegendPlacement = LegendPlacement.Outside;
        PlotModel.LegendPosition = LegendPosition.TopCenter;
        PlotModel.PlotAreaBorderColor = OxyColor.Parse("#999999");
        PlotModel.PlotMargins = new OxyThickness(50, 15, 30, 60);
        PlotModel.Title = "Temperature graph";

        var verticalAxis = new LinearAxis();
        verticalAxis.AbsoluteMinimum = 0;
        verticalAxis.AxisTitleDistance = 15;
        verticalAxis.MajorStep = 10;
        verticalAxis.Minimum = 0;
        verticalAxis.MinimumRange = 100;
        verticalAxis.MinorStep = 1;
        verticalAxis.Position = AxisPosition.Left;
        verticalAxis.Title = "Temperature (°C)";
        verticalAxis.TitleColor = OxyColors.Chocolate;
        verticalAxis.TitleFontSize = 12;
        verticalAxis.TitleFontWeight = FontWeights.Bold;
        PlotModel.Axes.Add(verticalAxis);
        
        var horizontalAxis = new LinearAxis();
        horizontalAxis.AbsoluteMinimum = 0;
        horizontalAxis.AxisTitleDistance = 10;
        horizontalAxis.MajorStep = 10;
        horizontalAxis.Minimum = 0;
        horizontalAxis.MinimumRange = 100;
        horizontalAxis.MinorStep = 1;
        horizontalAxis.Position = AxisPosition.Bottom;
        horizontalAxis.Title = "Time (s)";
        horizontalAxis.TitleColor = OxyColors.Chocolate;
        horizontalAxis.TitleFontSize = 12;
        horizontalAxis.TitleFontWeight = FontWeights.Bold;
        PlotModel.Axes.Add(horizontalAxis);
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