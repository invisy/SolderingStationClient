using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Collections;
using OxyPlot;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.Models;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ThermalProfileEditorWindowViewModel : ViewModelBase, IThermalProfileEditorWindowViewModel
{
    private readonly IMessageBoxService _messageBoxService;
    private readonly IThermalProfileService _thermalProfileService;
    private ThermalProfileViewModel? _selectedThermalProfile;

    private IList<uint> _thermalProfilesSoftDeleteList = new List<uint>();
   
    public IAvaloniaList<ThermalProfileViewModel> ThermalProfiles { get; } = new AvaloniaList<ThermalProfileViewModel>();

    public ThermalProfileViewModel? SelectedThermalProfile
    {
        get => _selectedThermalProfile;
        set => this.RaiseAndSetIfChanged(ref _selectedThermalProfile, value);
    }
    
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
    
    public ThermalProfileEditorWindowViewModel(
        IThermalProfileService thermalProfileService, 
        IMessageBoxService messageBoxService)
    {
        _thermalProfileService = thermalProfileService;
        _messageBoxService = messageBoxService;

        SaveCommand = ReactiveCommand.CreateFromTask(UpdateThermalProfiles);
        CloseCommand = ReactiveCommand.Create(() => new Unit());
    }

    public async Task Init()
    {
        await LoadAllProfiles();
    }
    
    public void Create()
    {
        var newThermalProfileVm = new ThermalProfileViewModel("New thermal profile");
        ThermalProfiles.Add(newThermalProfileVm);
        SelectedThermalProfile = newThermalProfileVm;
    }

    public void Remove()
    {
        if (SelectedThermalProfile == null)
            return;
        _thermalProfilesSoftDeleteList.Add(SelectedThermalProfile.Id);
        ThermalProfiles.Remove(SelectedThermalProfile);
        if(ThermalProfiles.Count != 0)
            SelectedThermalProfile = ThermalProfiles[0];
    }

    private async Task UpdateThermalProfiles()
    {
        try
        {
            var allThermalProfiles = ThermalProfiles.Select(profile => Map(profile)).ToList();

            foreach (var id in _thermalProfilesSoftDeleteList)
            {
                if (id != 0)
                    await _thermalProfileService.Remove(id);
            }
            _thermalProfilesSoftDeleteList.Clear();

            foreach (var profile in allThermalProfiles)
            {
                if(profile.Id != 0)
                    await _thermalProfileService.Remove(profile.Id);
                await _thermalProfileService.Add(profile);
            }
        }
        catch (Exception e)
        {
            await _messageBoxService.ShowMessageBoxWithLocalizedMessage("Localization.UnknownError", MessageBoxType.Error);
        }
    }

    private async Task LoadAllProfiles()
    {
        ThermalProfiles.Clear();
        var thermalProfiles =  await _thermalProfileService.GetAll();
        foreach (var thermalProfile in thermalProfiles)
            ThermalProfiles.Add(Map(thermalProfile));
        SelectedThermalProfile = ThermalProfiles.FirstOrDefault();
    }
    
    private ThermalProfileViewModel Map(ThermalProfile thermalProfile)
    {
        return new ThermalProfileViewModel(thermalProfile);
    }

    private ThermalProfile Map(ThermalProfileViewModel vm)
    {
        var controllerThermalProfiles = vm.ThermalProfileParts.Select(part => Map(part));
        return new ThermalProfile(vm.Id, vm.Name, controllerThermalProfiles);
    }
    
    private ControllerThermalProfile Map(ThermalProfilePartViewModel thermalProfile)
    {
        var points = thermalProfile.Curve.Points.Select(point => Map(point)).ToList();
        var result = new ControllerThermalProfile(0, thermalProfile.Name,
            (int)thermalProfile.Color.ToUint32(), points);

        return result;
    }

    private ThermalProfilePoint Map(DataPoint point)
    {
        return new ThermalProfilePoint(0, (float)point.X, (ushort)point.Y);
    }
}