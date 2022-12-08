using UIStatesStore.App.Models;
using UIStatesStore.Contracts;

namespace UIStatesStore.App.Observable
{
    public class AppErrorObservable : IAppObservable<AppErrorMessage>, IDisposable

    {
        private readonly IList<IObserver<AppErrorMessage>> _listeners = new List<IObserver<AppErrorMessage>>();

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

        public void Send(AppErrorMessage value)
        {
            if (_listeners.Count > 0)
                foreach (var listener in _listeners)
                    listener.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<AppErrorMessage> observer)
        {
            var Duplicated =
                _listeners.Where(obs => obs.GetType() == observer.GetType());

            if (Duplicated.Count() >  0)
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
