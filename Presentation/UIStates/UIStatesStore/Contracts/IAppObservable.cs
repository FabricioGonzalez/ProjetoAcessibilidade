using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UIStatesStore.Project.Observable;

namespace UIStatesStore.Contracts;
public interface IAppObservable<T> : IObservable<T>
{
    public void Send(T value);
    public IDisposable Subscribe(IObserver<T> observer);
}
