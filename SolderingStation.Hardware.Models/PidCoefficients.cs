namespace SolderingStation.Hardware.Models;

public class PidCoefficients
{
    public PidCoefficients(float kp, float ki, float kd)
    {
        Kp = kp;
        Ki = ki;
        Kd = kd;
    }

    public float Kp { get; }
    public float Ki { get; }
    public float Kd { get; }
}