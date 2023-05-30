namespace SolderingStationClient.Presentation.Services;

public interface IResourceProvider
{
    T GetResourceByName<T>(string name);
}