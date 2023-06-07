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
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private readonly IViewModelCreator _viewModelCreator;
    private readonly IJobStateService _jobStateService;

    private bool _isJobRunning;
    private float _jobProgress;

    public bool IsJobRunning
    {
        get => _isJobRunning;
        set
        {
            _isJobRunning = value;
            ConnectionViewModel.IsActive = !value;
            DevicesListViewModel.IsActive = !value;
            MainPlotViewModel.IsActive = !value;
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
        IViewModelCreator viewModelCreator,
        IJobStateService jobStateService
    )
    {
        _viewModelCreator = viewModelCreator;
        _jobStateService = Guard.Against.Null(jobStateService);
        LanguageSettingsViewModel = viewModelCreator.Create<ILanguageSettingsViewModel>();
        ConnectionViewModel = viewModelCreator.Create<IConnectionViewModel>();
        MainPlotViewModel = viewModelCreator.Create<IMainPlotViewModel>();
        DevicesListViewModel = viewModelCreator.Create<IDevicesListViewModel>();
        
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
        var thermalProfileEditorVm = _viewModelCreator.Create<IThermalProfileEditorWindowViewModel>();
        await thermalProfileEditorVm.Init();
        var result = await ShowThermalProfileEditorWindow.Handle(thermalProfileEditorVm);
    }
    
    public async Task OpenThermalProfileRunnerWindow()
    {
        var thermalProfileRunnerVm = _viewModelCreator.Create<IThermalProfileRunnerWindowViewModel>();
        var result = await ShowThermalProfileSelectorWindow.Handle(thermalProfileRunnerVm);
    }

    public Interaction<IThermalProfileEditorWindowViewModel, Unit> ShowThermalProfileEditorWindow { get; } = new();
    public Interaction<IThermalProfileRunnerWindowViewModel, Unit> ShowThermalProfileSelectorWindow { get; } = new();

    private void OnJobStarted(object? sender, JobStartedEventArgs? eventArgs)
    {
        IsJobRunning = true;
        eventArgs!.Job.ProgressUpdated += OnCurrentProgress;
        eventArgs.Job.StateChanged += OnStateChanged;
    }

    private void OnCurrentProgress(object? sender, JobProgressUpdatedEventArgs args)
    {
        JobProgress = args.CurrentProgress;
    }
    
    private void OnStateChanged(object? sender, JobStateChangedEventArgs args)
    {
        JobProgress = 0;
        IsJobRunning = false;
    }
}