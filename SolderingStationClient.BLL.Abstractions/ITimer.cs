namespace SolderingStationClient.BLL.Abstractions;

public interface ITimer : IDisposable
{
    public delegate void TimerIntervalElapsedEventHandler(object sender, DateTime dateTime);

    bool Enabled { get; set; }
    double Interval { get; set; }

    event TimerIntervalElapsedEventHandler TimerIntervalElapsed;

    void Start();
    void Stop();
}