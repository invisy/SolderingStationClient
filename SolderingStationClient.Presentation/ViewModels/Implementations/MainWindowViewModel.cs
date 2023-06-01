using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Avalonia.Collections;
using ReactiveUI;
using SolderingStationClient.Models;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private readonly IThermalProfileEditorWindowViewModel _thermalProfileEditorVm;
    
    public MainWindowViewModel(
        IThermalProfileEditorWindowViewModel thermalProfileEditorVm,
        ILanguageSettingsViewModel languageSettingsViewModel,
        IConnectionViewModel connectionViewModel,
        IMainPlotViewModel mainPlotViewModel,
        IDevicesListViewModel devicesListViewModel
    )
    {
        _thermalProfileEditorVm = thermalProfileEditorVm;
        LanguageSettingsViewModel = Guard.Against.Null(languageSettingsViewModel);
        ConnectionViewModel = Guard.Against.Null(connectionViewModel);
        MainPlotViewModel = Guard.Against.Null(mainPlotViewModel);
        DevicesListViewModel = Guard.Against.Null(devicesListViewModel);
        
        RxApp.MainThreadScheduler.Schedule(Init);
    }

    public ILanguageSettingsViewModel LanguageSettingsViewModel { get; }
    public IConnectionViewModel ConnectionViewModel { get; }
    public IMainPlotViewModel MainPlotViewModel { get; }
    public IDevicesListViewModel DevicesListViewModel { get; }

    public IAvaloniaList<Locale> LanguagesList { get; } = new AvaloniaList<Locale>();

    private async void Init()
    {
        await LanguageSettingsViewModel.Init();
        //await DevicesListViewModel.Init();
    }
    
    public async Task OpenThermalProfileEditorWindow()
    {
        var result = await ShowThermalProfileEditorWindow.Handle(_thermalProfileEditorVm);
    }

    public Interaction<IThermalProfileEditorWindowViewModel, Unit> ShowThermalProfileEditorWindow { get; } = new();
}