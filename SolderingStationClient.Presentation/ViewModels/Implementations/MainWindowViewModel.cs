using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Avalonia.Collections;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.BLL.Implementation.Extensions;
using SolderingStationClient.Models;
using SolderingStationClient.Models.Jobs;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private readonly IViewModelCreator _viewModelCreator;
    private readonly IJobStateService _jobStateService;
    private readonly IThermalProfileProcessingService _thermalProfileProcessingService;

    private IJob? _job;
    private bool _isJobRunning;
    private float _jobProgress;

    public bool IsJobRunning
    {
        get => _isJobRunning;
        set
        {
            ConnectionViewModel.IsActive = !value;
            DevicesListViewModel.IsActive = !value;
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
        IScheduler scheduler,
        IViewModelCreator viewModelCreator,
        IJobStateService jobStateService,
        IThermalProfileProcessingService thermalProfileProcessingService
    )
    {
        _viewModelCreator = Guard.Against.Null(viewModelCreator);
        _jobStateService = Guard.Against.Null(jobStateService);
        _thermalProfileProcessingService = Guard.Against.Null(thermalProfileProcessingService);
        LanguageSettingsViewModel = viewModelCreator.Create<ILanguageSettingsViewModel>();
        ConnectionViewModel = viewModelCreator.Create<IConnectionViewModel>();
        MainPlotViewModel = viewModelCreator.Create<IMainPlotViewModel>();
        DevicesListViewModel = viewModelCreator.Create<IDevicesListViewModel>();

        scheduler.Schedule(Init);
    }

    public ILanguageSettingsViewModel LanguageSettingsViewModel { get; }
    public IConnectionViewModel ConnectionViewModel { get; }
    public IDevicesListViewModel DevicesListViewModel { get; }
    public IMainPlotViewModel MainPlotViewModel { get; }

    public IAvaloniaList<Locale> LanguagesList { get; } = new AvaloniaList<Locale>();
    
    public Interaction<IThermalProfileEditorWindowViewModel, Unit> ShowThermalProfileEditorWindow { get; } = new();
    public Interaction<IThermalProfileRunnerWindowViewModel, Unit> ShowThermalProfileRunnerWindow { get; } = new();

    public async void Init()
    {
        await LanguageSettingsViewModel.Init();
        await DevicesListViewModel.Init();
        ConnectionViewModel.Init();
        MainPlotViewModel.Init();

        IsJobRunning = false;
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
        if (_job != null)
        {
            _job.Cancel();
            return;
        }

        var thermalProfileRunnerVm = _viewModelCreator.Create<IThermalProfileRunnerWindowViewModel>();
        await ShowThermalProfileRunnerWindow.Handle(thermalProfileRunnerVm);
    }

    private void OnJobStarted(object? sender, JobStartedEventArgs? eventArgs)
    {
        IsJobRunning = true;
        _job = eventArgs!.Job;
        _job.ProgressUpdated += OnCurrentProgress;
        _job.StateChanged += OnStateChanged;
    }

    private void OnCurrentProgress(object? sender, JobProgressUpdatedEventArgs args)
    {
        JobProgress = args.CurrentProgress;
    }
    
    private void OnStateChanged(object? sender, JobStateChangedEventArgs args)
    {
        if(!args.JobState.IsCompleted())
            return;
        
        JobProgress = 0;
        IsJobRunning = false;
        
        _job!.ProgressUpdated -= OnCurrentProgress;
        _job.StateChanged -= OnStateChanged;
        _job = null;
    }
}