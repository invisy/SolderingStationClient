namespace SolderingStationClient.Models.TemperatureControllers;

/// <summary>
///     The PID coefficients configuration.
/// </summary>
public class PidTemperatureController : TemperatureController
{
    /// <summary>
    ///     Initializes a new instance of <see cref="PidTemperatureController" />.
    /// </summary>
    /// <param name="tController">Temperature controller info.</param>
    /// <param name="coefficients">PID controller coefficients</param>
    public PidTemperatureController(TemperatureController tController, PidControllerCoefficients coefficients)
        : base(tController.Key, tController.CurrentTemperature, tController.DesiredTemperature)
    {
        PidCoefficients = coefficients;
    }

    /// <summary>
    ///     Returns coefficients.
    /// </summary>
    public PidControllerCoefficients PidCoefficients { get; }
}