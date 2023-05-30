namespace SolderingStationClient.Models.TemperatureControllers;

public class PidControllerCoefficients
{
    public PidControllerCoefficients(float kp, float ki, float kd)
    {
        Kp = kp;
        Ki = ki;
        Kd = kd;
    }

    /// <summary>
    ///     Returns a proportional coefficient of the PID device.
    /// </summary>
    public float Kp { get; }

    /// <summary>
    ///     Returns a integral coefficient of the PID device.
    /// </summary>
    public float Ki { get; }

    /// <summary>
    ///     Returns a differential coefficient of the PID device.
    /// </summary>
    public float Kd { get; }
}