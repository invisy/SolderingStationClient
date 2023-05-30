using SolderingStationClient.Models;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;

public interface IDeviceViewModelFactory
{
    IDeviceViewModel Create(Device device);
}