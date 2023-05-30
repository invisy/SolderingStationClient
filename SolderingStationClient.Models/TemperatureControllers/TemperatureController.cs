namespace SolderingStationClient.Models.TemperatureControllers;

/// <summary>
///     The temperature controller values.
/// </summary>
public class TemperatureController
{
    /// <summary>
    ///     Initializes a new instance of <see cref="TemperatureController" />.
    /// </summary>
    /// ///
    /// <param name="key">Composite id of temperature controller.</param>
    /// <param name="currentTemperature">Current temperature.</param>
    /// <param name="desiredTemperature">Desired temperature</param>
    public TemperatureController(TemperatureControllerKey key, ushort currentTemperature, ushort desiredTemperature)
    {
        Key = key;
        CurrentTemperature = currentTemperature;
        DesiredTemperature = desiredTemperature;
    }

    /// <summary>
    ///     Internal id of PID in device.
    /// </summary>
    public TemperatureControllerKey Key { get; }

    /// <summary>
    ///     Returns the current temperature of the heater.
    /// </summary>
    public ushort CurrentTemperature { get; }

    /// <summary>
    ///     Returns the expected (set) temperature of the heater.
    /// </summary>
    public ushort DesiredTemperature { get; }
}