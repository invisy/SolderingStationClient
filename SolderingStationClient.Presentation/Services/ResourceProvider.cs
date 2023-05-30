using System;
using Avalonia;

namespace SolderingStationClient.Presentation.Services;

public class ResourceProvider : IResourceProvider
{
    public T GetResourceByName<T>(string name)
    {
        if (Application.Current!.Resources.TryGetResource(name, out var message))
        {
            if(message is T stringResource)
                return stringResource;
        }
        
        throw new ArgumentException(name);
    }
}