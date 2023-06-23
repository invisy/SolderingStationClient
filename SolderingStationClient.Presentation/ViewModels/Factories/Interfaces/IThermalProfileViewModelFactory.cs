using SolderingStationClient.Models;
using SolderingStationClient.Presentation.ViewModels.Implementations;

namespace SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;

public interface IThermalProfileViewModelFactory
{
    ThermalProfileViewModel Create(string name);
    ThermalProfileViewModel Create(ThermalProfile thermalProfile);
}