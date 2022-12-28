using UIStatesStore.Contracts;
using UIStatesStore.Project.Models;

namespace UIStatesStore.Project.Observable
{
    public class ProjectStateObservable : IAppObservable<ProjectModel>, IDisposable

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
                if (value.ProjectPath.Length > 0)
                    foreach (var listener in _listeners)
                        listener.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<ProjectModel> observer)
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
}
