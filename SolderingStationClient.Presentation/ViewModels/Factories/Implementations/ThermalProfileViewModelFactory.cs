using SolderingStationClient.Models;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Factories.Interfaces;
using SolderingStationClient.Presentation.ViewModels.Implementations;

namespace SolderingStationClient.Presentation.ViewModels.Factories.Implementations;

public class ThermalProfileViewModelFactory : IThermalProfileViewModelFactory
{
    private readonly IPlotModelFactory _plotModelFactory;
    private readonly IResourceProvider _resourceProvider;
    
    public ThermalProfileViewModelFactory(IPlotModelFactory plotModelFactory, IResourceProvider resourceProvider)
    {
        _plotModelFactory = plotModelFactory;
        _resourceProvider = resourceProvider;
    }
    
    public ThermalProfileViewModel Create(string name)
    {
        return new ThermalProfileViewModel(name, _plotModelFactory, _resourceProvider);
    }

    public ThermalProfileViewModel Create(ThermalProfile thermalProfile)
    {
        return new ThermalProfileViewModel(thermalProfile, _plotModelFactory, _resourceProvider);
    }
}