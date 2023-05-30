using System;
using System.Threading.Tasks;

namespace SolderingStationClient.Presentation.Services;

public interface IApplicationDispatcher
{
    void Dispatch(Action action);
    Task DispatchAsync(Func<Task> task);
}