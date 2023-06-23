using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;

namespace SolderingStationClient.Presentation.Services;

public class ResourceProvider : IResourceProvider
{
    public event Action? ResourcesChanged;

    public ResourceProvider()
    {
        Application.Current!.ResourcesChanged += OnResourceChanged;
    }

    public T GetResourceByName<T>(string name)
    {
        if (Application.Current!.Resources.TryGetResource(name, out var message))
        {
            if(message is T stringResource)
                return stringResource;
        }
        
        throw new ArgumentException(name);
        
    }
    
    private async void OnResourceChanged(object? sender, ResourcesChangedEventArgs? args)
    {
        await Task.Delay(100);
        ResourcesChanged?.Invoke();
    }
}