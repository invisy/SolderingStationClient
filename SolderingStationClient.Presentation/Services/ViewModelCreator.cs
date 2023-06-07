using System;
using SolderingStationClient.Presentation.ViewModels.Interfaces;
using Splat;

namespace SolderingStationClient.Presentation.Services;

public class ViewModelCreator : IViewModelCreator
{
    private readonly IReadonlyDependencyResolver _resolver;
    
    public ViewModelCreator(IReadonlyDependencyResolver resolver)
    {
        _resolver = resolver;
    }
    
    public T Create<T>() where T : IViewModelBase
    {
        var service = _resolver.GetService<T>();
        if (service == null)
            throw new NullReferenceException($"ViewModel {typeof(T)} was not registered!");
        return service;
    }
}