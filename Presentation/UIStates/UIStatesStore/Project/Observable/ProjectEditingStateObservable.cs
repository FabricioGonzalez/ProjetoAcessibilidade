using UIStatesStore.Contracts;
using UIStatesStore.Project.Models;

namespace UIStatesStore.Project.Observable;

public class ProjectEditingStateObservable : IAppObservable<ProjectEditingModel>, IDisposable

{
    private readonly IList<IObserver<ProjectEditingModel>> _listeners = new List<IObserver<ProjectEditingModel>>();

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Console.WriteLine("Dispose!");
            }
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    public void Send(ProjectEditingModel value)
    {
        if (_listeners.Count > 0)
            foreach (var listener in _listeners)
                listener.OnNext(value);
    }

    public IDisposable Subscribe(IObserver<ProjectEditingModel> observer)
    {
        var Duplicated =
            _listeners.Where(obs => obs.GetType() == observer.GetType());

        if (Duplicated.Count() > 0)
        {
            foreach (var item in Duplicated.ToList())
            {
                _listeners.Remove(item);
            }
        }
        _listeners.Add(observer);

        return this;
    }
}
