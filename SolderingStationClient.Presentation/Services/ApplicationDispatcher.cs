using System;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace SolderingStationClient.Presentation.Services;

public class ApplicationDispatcher : IApplicationDispatcher
{
    private static Dispatcher Dispatcher => Dispatcher.UIThread;
    public void Dispatch(Action action) => Dispatcher.Post(action);
    public Task DispatchAsync(Func<Task> task) => Dispatcher.InvokeAsync(task);
}