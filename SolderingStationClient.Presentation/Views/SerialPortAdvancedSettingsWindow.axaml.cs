using System;
using System.Reactive;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SolderingStationClient.Presentation.ViewModels.Implementations;

namespace SolderingStationClient.Presentation.Views;

public class SerialPortAdvancedSettingsWindow : ReactiveWindow<SerialPortAdvancedSettingsWindowViewModel>
{
    public SerialPortAdvancedSettingsWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        this.WhenActivated(d =>
        {
            d(ViewModel!.ApplyCommand.Subscribe(Close));
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