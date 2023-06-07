using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.Views;

public class MainWindow : ReactiveWindow<IMainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        
        this.WhenActivated(d =>
            d(ViewModel!.ConnectionViewModel.ShowSerialPortAdvancedSettingsWindow.RegisterHandler(
                DoShowSerialPortAdvancedSettingsWindow))
        );
        
        this.WhenActivated(d =>
            d(ViewModel!.ShowThermalProfileEditorWindow.RegisterHandler(
                DoShowThermalProfileEditorWindow))
        );
        
        this.WhenActivated(d =>
            d(ViewModel!.ShowThermalProfileSelectorWindow.RegisterHandler(
                DoShowThermalProfileRunnerWindow))
        );
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private async Task DoShowSerialPortAdvancedSettingsWindow(
        InteractionContext<ISerialPortAdvancedSettingsWindowViewModel, Unit> interaction)
    {
        var window = new SerialPortAdvancedSettingsWindow { DataContext = interaction.Input };
        await window.ShowDialog(this);
        interaction.SetOutput(Unit.Default);
    }

    private async Task DoShowThermalProfileEditorWindow(
        InteractionContext<IThermalProfileEditorWindowViewModel, Unit> interaction)
    {
        var window = new ThermalProfileEditorWindow()
        {
            DataContext = interaction.Input
        };

        await window.ShowDialog(this);
        interaction.SetOutput(Unit.Default);
    }
    
    private async Task DoShowThermalProfileRunnerWindow(
        InteractionContext<IThermalProfileRunnerWindowViewModel, Unit> interaction)
    {
        var window = new ThermalProfileRunnerWindow()
        {
            DataContext = interaction.Input
        };

        await window.ShowDialog(this);
        interaction.SetOutput(Unit.Default);
    }
}