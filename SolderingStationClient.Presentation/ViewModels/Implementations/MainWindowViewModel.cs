using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Avalonia.Collections;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.Jobs;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private readonly IThermalProfileEditorWindowViewModel _thermalProfileEditorVm;
    private readonly IJobStateService _jobStateService;

    private bool _isJobRunning;

    public bool IsJobRunning
    {
        get => _isJobRunning;
        set
        {
            _isJobRunning = value;
            ConnectionViewModel.IsActive = false;
            DevicesListViewModel.IsActive = false;
            MainPlotViewModel.IsActive = false;
        }
    }

    public MainWindowViewModel(
        IThermalProfileEditorWindowViewModel thermalProfileEditorVm,
        ILanguageSettingsViewModel languageSettingsViewModel,
        IConnectionViewModel connectionViewModel,
        IMainPlotViewModel mainPlotViewModel,
        IDevicesListViewModel devicesListViewModel,
        IJobStateService jobStateService
    )
    {
        _jobStateService = jobStateService;
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
        await DevicesListViewModel.Init();
        _jobStateService.JobStarted += OnJobStarted;
    }
    
    public async Task OpenThermalProfileEditorWindow()
    {
        var result = await ShowThermalProfileEditorWindow.Handle(_thermalProfileEditorVm);
    }

    public Interaction<IThermalProfileEditorWindowViewModel, Unit> ShowThermalProfileEditorWindow { get; } = new();

    private void OnJobStarted(object? sender, JobStartedEventArgs? eventArgs)
    {
        IsJobRunning = true;
    }
}