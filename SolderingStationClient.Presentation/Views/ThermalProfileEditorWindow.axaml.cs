using System;
using System.Reactive;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.Views;

public partial class ThermalProfileEditorWindow : ReactiveWindow<IThermalProfileEditorWindowViewModel>
{
    public ThermalProfileEditorWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        
        this.WhenActivated(d =>
        {
            d(ViewModel!.SaveCommand.Subscribe(Close));
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