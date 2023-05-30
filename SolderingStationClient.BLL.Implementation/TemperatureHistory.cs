using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.Models.TemperatureControllers;

namespace SolderingStationClient.BLL.Implementation;

public class TemperatureHistoryTracker : ITemperatureHistoryTracker
{
    private readonly ConcurrentDictionary<TemperatureControllerKey, Collection<TemperatureMeasurement>>
        _temperatureHistory =
            new();

    public IEnumerable<TemperatureMeasurement> GetControllerHistory(TemperatureControllerKey controllerKey)
    {
        if (!_temperatureHistory.TryGetValue(controllerKey, out var controllerTemperatureHistory))
            yield break;

        foreach (var measurement in controllerTemperatureHistory)
            yield return measurement;
    }

    public void AddMeasurement(TemperatureControllerKey controllerKey, TemperatureMeasurement temperatureMeasurement)
    {
        if (_temperatureHistory.TryGetValue(controllerKey, out var controllerTemperatureHistory))
        {
            controllerTemperatureHistory.Add(temperatureMeasurement);
        }
        else
        {
            var newControllerHistory = new Collection<TemperatureMeasurement> { temperatureMeasurement };
            _temperatureHistory.TryAdd(controllerKey, newControllerHistory);
        }
    }

    public void RemoveControllerHistory(TemperatureControllerKey controllerKey)
    {
        _temperatureHistory.Remove(controllerKey, out _);
    }

    public void Clear()
    {
        _temperatureHistory.Clear();
    }
}