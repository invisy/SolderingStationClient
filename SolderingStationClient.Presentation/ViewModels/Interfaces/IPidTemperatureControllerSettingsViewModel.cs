using System.Threading.Tasks;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface IPidTemperatureControllerSettingsViewModel : ITemperatureControllerSettingsViewModel
{
    float PidKp { get; set; }
    float PidKi { get; set; }
    float PidKd { get; set; }
    float NewPidKp { get; set; }
    float NewPidKi { get; set; }
    float NewPidKd { get; set; }
    new Task UpdateViewModel();
    Task SetKp();
    Task SetKi();
    Task SetKd();
}