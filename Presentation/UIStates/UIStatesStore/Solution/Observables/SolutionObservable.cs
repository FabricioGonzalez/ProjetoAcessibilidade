using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UIStatesStore.Contracts;
using UIStatesStore.Project.Models;
using UIStatesStore.Solution.Models;

namespace UIStatesStore.Solution.Observables;
public class SolutionObservable : IAppObservable<ProjectSolutionModel>, IDisposable
{
    private readonly IList<IObserver<ProjectSolutionModel>> _listeners = new List<IObserver<ProjectSolutionModel>>();

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

    public void Send(ProjectSolutionModel value)
    {
        if (_listeners.Count > 0)
           
                foreach (var listener in _listeners)
                    listener.OnNext(value);
    }

    public IDisposable Subscribe(IObserver<ProjectSolutionModel> observer)
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
