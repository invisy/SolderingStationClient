using System.Threading.Tasks;
using Ardalis.GuardClauses;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class PidTemperatureControllerSettingsViewModel : TemperatureControllerSettingsViewModel,
    IPidTemperatureControllerSettingsViewModel
{
    private readonly IPidTemperatureControllerService _pidTemperatureControllerService;

    private float _newPidKd;

    private float _newPidKi;

    private float _newPidKp;

    private float _pidKd;

    private float _pidKi;

    private float _pidKp;

    public PidTemperatureControllerSettingsViewModel(IPidTemperatureControllerService pidTemperatureControllerService,
        TemperatureControllerKey key) : base(pidTemperatureControllerService, key)
    {
        _pidTemperatureControllerService = Guard.Against.Null(pidTemperatureControllerService);
    }

    public float PidKp
    {
        get => _pidKp;
        set => this.RaiseAndSetIfChanged(ref _pidKp, value);
    }

    public float PidKi
    {
        get => _pidKi;
        set => this.RaiseAndSetIfChanged(ref _pidKi, value);
    }

    public float PidKd
    {
        get => _pidKd;
        set => this.RaiseAndSetIfChanged(ref _pidKd, value);
    }

    public float NewPidKp
    {
        get => _newPidKp;
        set => this.RaiseAndSetIfChanged(ref _newPidKp, value);
    }

    public float NewPidKi
    {
        get => _newPidKi;
        set => this.RaiseAndSetIfChanged(ref _newPidKi, value);
    }

    public float NewPidKd
    {
        get => _newPidKd;
        set => this.RaiseAndSetIfChanged(ref _newPidKd, value);
    }

    public async Task SetKp()
    {
        await _pidTemperatureControllerService.SetPidKp(_key, NewPidKp);
        await UpdateCoefficients();
    }

    public async Task SetKi()
    {
        await _pidTemperatureControllerService.SetPidKi(_key, NewPidKi);
        await UpdateCoefficients();
    }

    public async Task SetKd()
    {
        await _pidTemperatureControllerService.SetPidKd(_key, NewPidKd);
        await UpdateCoefficients();
    }

    public new async Task UpdateViewModel()
    {
        await base.UpdateViewModel();
        await UpdateCoefficients();
    }

    private async Task UpdateCoefficients()
    {
        var pidCoefs = await _pidTemperatureControllerService.GetCoefficients(_key);
        PidKp = pidCoefs.Kp;
        PidKi = pidCoefs.Ki;
        PidKd = pidCoefs.Kd;
    }
}