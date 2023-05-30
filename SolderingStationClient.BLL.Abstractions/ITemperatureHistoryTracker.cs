using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Abstractions;

public interface ITemperatureHistoryTracker
{
    IEnumerable<TemperatureMeasurement> GetControllerHistory(TemperatureControllerKey controllerKey);
    void AddMeasurement(TemperatureControllerKey controllerKey, TemperatureMeasurement temperatureMeasurement);
    void RemoveControllerHistory(TemperatureControllerKey controllerKey);
    void Clear();
}