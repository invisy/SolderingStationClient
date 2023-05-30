using System;

namespace SolderingStationClient.Presentation.Services;

public interface IResourceProvider
{
    event Action? ResourcesChanged;
    T GetResourceByName<T>(string name);
}