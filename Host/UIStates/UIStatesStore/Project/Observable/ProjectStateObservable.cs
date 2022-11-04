using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

using UIStatesStore.Contracts;
using UIStatesStore.Project.Models;

namespace UIStatesStore.Project.Observable
{
    public class ProjectStateObservable : IAppObservable<ProjectModel>,IDisposable

    {
        private readonly IList<IObserver<ProjectModel>> _listeners = new List<IObserver<ProjectModel>>();

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

        public void Send(ProjectModel value)
        {
            if (_listeners.Count > 0)
                foreach (var listener in _listeners)
                    listener.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<ProjectModel> observer)
        {
            var isDuplicated = _listeners.Any(obs => obs.GetType() == observer.GetType());
            if (!isDuplicated)
            _listeners.Add(observer);
            return this;
        }
    }
}
