using System.Threading.Tasks;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface ITemperatureControllerSettingsViewModel : IViewModelBase
{
    public byte ControllerId { get; }
    ushort CurrentTemperature { get; set; }
    ushort DesiredTemperature { get; set; }
    ushort NewDesiredTemperature { get; set; }
    Task UpdateViewModel();
    Task SetTemperature();
}