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
    private readonly IThermalProfileRunnerViewModel _thermalProfileRunnerVm;
    private readonly IJobStateService _jobStateService;

    private bool _isJobRunning;
    private float _jobProgress;

    public bool IsJobRunning
    {
        get => _isJobRunning;
        set
        {
            _isJobRunning = value;
            ConnectionViewModel.IsActive = false;
            DevicesListViewModel.IsActive = false;
            MainPlotViewModel.IsActive = false;
            this.RaiseAndSetIfChanged(ref _isJobRunning, value);
        }
    }
    
    public float JobProgress
    {
        get => _jobProgress;
        set
        {
            this.RaiseAndSetIfChanged(ref _jobProgress, value);
        }
    }

    public MainWindowViewModel(
        IThermalProfileEditorWindowViewModel thermalProfileEditorVm,
        ILanguageSettingsViewModel languageSettingsViewModel,
        IConnectionViewModel connectionViewModel,
        IMainPlotViewModel mainPlotViewModel,
        IDevicesListViewModel devicesListViewModel,
        IThermalProfileRunnerViewModel thermalProfileRunnerVm,
        IJobStateService jobStateService
    )
    {
        _jobStateService = Guard.Against.Null(jobStateService);
        _thermalProfileEditorVm = Guard.Against.Null(thermalProfileEditorVm);
        _thermalProfileRunnerVm = Guard.Against.Null(thermalProfileRunnerVm);
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
    
    public async Task OpenThermalProfileRunnerWindow()
    {
        var result = await ShowThermalProfileSelectorWindow.Handle(_thermalProfileRunnerVm);
    }

    public Interaction<IThermalProfileEditorWindowViewModel, Unit> ShowThermalProfileEditorWindow { get; } = new();
    public Interaction<IThermalProfileRunnerViewModel, Unit> ShowThermalProfileSelectorWindow { get; } = new();

    private void OnJobStarted(object? sender, JobStartedEventArgs? eventArgs)
    {
        IsJobRunning = true;
    }
}