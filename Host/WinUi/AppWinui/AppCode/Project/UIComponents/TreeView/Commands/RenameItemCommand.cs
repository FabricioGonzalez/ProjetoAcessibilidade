using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

using ReactiveUI;

namespace AppWinui.AppCode.Project.UIComponents.TreeView.Commands;
public class RenameItemCommand : ReactiveCommandBase<Unit, Unit>
{
    public override IObservable<bool> CanExecute => throw new NotImplementedException();

    public override IObservable<bool> IsExecuting => throw new NotImplementedException();

    public override IObservable<Exception> ThrownExceptions => throw new NotImplementedException();

    public override IObservable<Unit> Execute(Unit parameter) => throw new NotImplementedException();
    public override IObservable<Unit> Execute() => throw new NotImplementedException();
    public override IDisposable Subscribe(IObserver<Unit> observer) => throw new NotImplementedException();
    protected override void Dispose(bool disposing) => throw new NotImplementedException();
}
