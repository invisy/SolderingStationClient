using System;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.Views;

public partial class ThermalProfileRunnerWindow : ReactiveWindow<IThermalProfileRunnerWindowViewModel>
{
    public ThermalProfileRunnerWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.WhenActivated(d =>
        {
            d(ViewModel!.StartCommand.Subscribe(Close));
            d(ViewModel!.CloseCommand.Subscribe(Close));
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void Close(Unit unit)
    {
        Close();
    }
}