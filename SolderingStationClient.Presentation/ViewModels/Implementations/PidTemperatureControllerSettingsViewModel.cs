using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Invisy.SerialCommunicationProtocol.Exceptions;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.Models;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class PidTemperatureControllerSettingsViewModel : TemperatureControllerSettingsViewModel,
    IPidTemperatureControllerSettingsViewModel
{
    private readonly IMessageBoxService _messageBoxService;
    private readonly IPidTemperatureControllerService _pidTemperatureControllerService;

    private float _newPidKd;

    private float _newPidKi;

    private float _newPidKp;

    private float _pidKd;

    private float _pidKi;

    private float _pidKp;

    public PidTemperatureControllerSettingsViewModel(
        IMessageBoxService messageBoxService,
        IPidTemperatureControllerService pidTemperatureControllerService,
        TemperatureControllerKey key) : base(messageBoxService, pidTemperatureControllerService, key)
    {
        _messageBoxService = Guard.Against.Null(messageBoxService);
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

    [Range(0, 100, ErrorMessage = "Value must be between {1} and {2}.")]
    public float NewPidKp
    {
        get => _newPidKp;
        set => this.RaiseAndSetIfChanged(ref _newPidKp, value);
    }

    [Range(0, 100, ErrorMessage = "Value must be between {1} and {2}.")]
    public float NewPidKi
    {
        get => _newPidKi;
        set => this.RaiseAndSetIfChanged(ref _newPidKi, value);
    }

    [Range(0, 100, ErrorMessage = "Value must be between {1} and {2}.")]
    public float NewPidKd
    {
        get => _newPidKd;
        set => this.RaiseAndSetIfChanged(ref _newPidKd, value);
    }

    public async Task SetKp()
    {
        try
        {
            await _pidTemperatureControllerService.SetPidKp(_key, NewPidKp);
        }
        catch (CommandException e)
        {
            await _messageBoxService.ShowMessageBoxWithLocalizedMessage("Localization.ConnectionLost", 
                MessageBoxType.Error, _key.DeviceId);
        }
        
        await UpdateCoefficients();
    }

    public async Task SetKi()
    {
        try
        {
            await _pidTemperatureControllerService.SetPidKi(_key, NewPidKi);
        }
        catch (CommandException e)
        {
            await _messageBoxService.ShowMessageBoxWithLocalizedMessage("Localization.ConnectionLost", 
                MessageBoxType.Error, _key.DeviceId);
        }
        await UpdateCoefficients();
    }

    public async Task SetKd()
    {
        try
        {
            await _pidTemperatureControllerService.SetPidKd(_key, NewPidKd);
        }
        catch (CommandException e)
        {
            await _messageBoxService.ShowMessageBoxWithLocalizedMessage("Localization.ConnectionLost", 
                MessageBoxType.Error, _key.DeviceId);
        }
        await UpdateCoefficients();
    }

    public new async Task UpdateViewModel()
    {
        await base.UpdateViewModel();
        await UpdateCoefficients();
    }

    private async Task UpdateCoefficients()
    {
        try
        {
            var pidCoefs = await _pidTemperatureControllerService.GetCoefficients(_key);
            PidKp = pidCoefs.Kp;
            PidKi = pidCoefs.Ki;
            PidKd = pidCoefs.Kd;
        }
        catch (CommandException e)
        {
            await _messageBoxService.ShowMessageBoxWithLocalizedMessage("Localization.ConnectionLost", 
                MessageBoxType.Error, _key.DeviceId);
        }
        
    }
}