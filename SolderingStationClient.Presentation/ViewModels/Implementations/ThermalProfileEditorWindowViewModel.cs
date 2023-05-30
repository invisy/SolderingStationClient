using System.Reactive;
using Avalonia.Collections;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ThermalProfileEditorWindowViewModel : ViewModelBase, IThermalProfileEditorWindowViewModel
{
    private readonly IThermalProfileService _thermalProfileService;
    private ThermalProfileViewModel? _selectedThermalProfile;
   
    public IAvaloniaList<ThermalProfileViewModel> ThermalProfiles { get; } = new AvaloniaList<ThermalProfileViewModel>();

    public ThermalProfileViewModel? SelectedThermalProfile
    {
        get => _selectedThermalProfile;
        set => this.RaiseAndSetIfChanged(ref _selectedThermalProfile, value);
    }
    
    public ThermalProfileEditorWindowViewModel(IThermalProfileService thermalProfileService)
    {
        _thermalProfileService = thermalProfileService;

        var thermalProfiles = _thermalProfileService.GetAll();
        foreach (var thermalProfile in thermalProfiles)
            ThermalProfiles.Add(GetVmFromModel(thermalProfile));
        
        if(ThermalProfiles.Count != 0)
            SelectedThermalProfile = ThermalProfiles[0];
    }
    
    public void Create()
    {
        var newThermalProfileVm = new ThermalProfileViewModel("New thermal profile");
        ThermalProfiles.Add(newThermalProfileVm);
    }

    public void Remove()
    {
        if (SelectedThermalProfile == null)
            return;
        ThermalProfiles.Remove(SelectedThermalProfile);
        if(ThermalProfiles.Count != 0)
            SelectedThermalProfile = ThermalProfiles[0];
    }

    public ReactiveCommand<Unit, Unit> SaveCommand { get; } = ReactiveCommand.Create(() =>
    {
        return Unit.Default;
    });

    public ReactiveCommand<Unit, Unit> CloseCommand { get; } = ReactiveCommand.Create(() => Unit.Default);

    private ThermalProfileViewModel GetVmFromModel(ThermalProfile thermalProfile)
    {
        return new ThermalProfileViewModel(thermalProfile);
    }
}