using System.Timers;
using SolderingStationClient.BLL.Abstractions;

namespace SolderingStationClient.BLL.Implementation;

public class Timer : ITimer
{
    private Dictionary<ITimer.TimerIntervalElapsedEventHandler, List<ElapsedEventHandler>> _handlers = new();
    private bool _isDisposed;
    private System.Timers.Timer _timer;

    public Timer()
    {
        _timer = new System.Timers.Timer();
    }

    public bool Enabled
    {
        get => _timer.Enabled;
        set => _timer.Enabled = value;
    }

    public double Interval
    {
        get => _timer.Interval;
        set => _timer.Interval = value;
    }

    public event ITimer.TimerIntervalElapsedEventHandler TimerIntervalElapsed
    {
        add
        {
            var internalHandler =
                (ElapsedEventHandler)((sender, args) => { value.Invoke(sender!, args.SignalTime); });

            if (!_handlers.ContainsKey(value)) _handlers.Add(value, new List<ElapsedEventHandler>());

            _handlers[value].Add(internalHandler);

            _timer.Elapsed += internalHandler;
        }

        remove
        {
            _timer.Elapsed -= _handlers[value].Last();

            _handlers[value].RemoveAt(_handlers[value].Count - 1);

            if (!_handlers[value].Any()) _handlers.Remove(value);
        }
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing && _handlers.Any())
        {
            foreach (var internalHandlers in _handlers.Values)
                if (internalHandlers?.Any() ?? false)
                    internalHandlers.ForEach(handler => _timer.Elapsed -= handler);

            _timer.Dispose();
            _timer = null!;
            _handlers.Clear();
            _handlers = null!;
        }

        _isDisposed = true;
    }

    ~Timer()
    {
        Dispose(false);
    }
}