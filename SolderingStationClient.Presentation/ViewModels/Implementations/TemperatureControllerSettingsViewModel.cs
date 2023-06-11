using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models.TemperatureControllers;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class TemperatureControllerSettingsViewModel : ViewModelBase, ITemperatureControllerSettingsViewModel
{
    protected readonly TemperatureControllerKey _key;
    private readonly ITemperatureControllerService _temperatureControllerService;

    private ushort _currentTemperature;
    private ushort _desiredTemperature;
    private ushort _newDesiredTemperature;

    public TemperatureControllerSettingsViewModel(
        ITemperatureControllerService temperatureControllerService,
        TemperatureControllerKey key)
    {
        _temperatureControllerService = Guard.Against.Null(temperatureControllerService);
        _key = key;
    }

    public byte ControllerId => _key.ChannelId;
    
    public ushort CurrentTemperature
    {
        get => _currentTemperature;
        set => this.RaiseAndSetIfChanged(ref _currentTemperature, value);
    }

    public ushort DesiredTemperature
    {
        get => _desiredTemperature;
        set => this.RaiseAndSetIfChanged(ref _desiredTemperature, value);
    }

    [Range(0, 500, ErrorMessage = "Value must be between {1} and {2}.")]
    public ushort NewDesiredTemperature
    {
        get => _newDesiredTemperature;
        set => this.RaiseAndSetIfChanged(ref _newDesiredTemperature, value);
    }

    public async Task SetTemperature()
    {
        await _temperatureControllerService.SetDesiredTemperature(_key, _newDesiredTemperature);
        await UpdateViewModel();
    }

    public async Task UpdateViewModel()
    {
        var controller = await _temperatureControllerService.GetTemperatureController(_key);
        CurrentTemperature = controller.CurrentTemperature;
        DesiredTemperature = controller.DesiredTemperature;
    }
}