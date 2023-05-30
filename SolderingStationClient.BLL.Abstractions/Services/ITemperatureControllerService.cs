using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface ITemperatureControllerService
{
    Task<TemperatureController> GetTemperatureController(TemperatureControllerKey controllerKey);
    Task SetDesiredTemperature(TemperatureControllerKey controllerKey, ushort value);
}