using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Abstractions.Services;

public interface IPidTemperatureControllerService : ITemperatureControllerService
{
    Task<PidTemperatureController> GetPidTemperatureController(TemperatureControllerKey controllerKey);
    Task<PidControllerCoefficients> GetCoefficients(TemperatureControllerKey controllerKey);
    Task SetPidKp(TemperatureControllerKey controllerKey, float value);
    Task SetPidKi(TemperatureControllerKey controllerKey, float value);
    Task SetPidKd(TemperatureControllerKey controllerKey, float value);
}