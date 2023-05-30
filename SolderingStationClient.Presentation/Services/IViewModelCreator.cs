using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.Services;

public interface IViewModelCreator
{
    T Create<T>() where T : IViewModelBase;
}