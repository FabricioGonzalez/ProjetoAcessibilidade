using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

using ReactiveUI;

namespace AppRestructure.Home.ViewModels;
public class HomeViewModel : ReactiveObject
{
    private string theText;
    public string TheText
    {
        get => theText;
        set => this.RaiseAndSetIfChanged(ref theText, value);
    }

    public ReactiveCommand<Unit, Unit> TheTextCommand
    {
        get;
    }

    public HomeViewModel()
    {
        TheTextCommand = ReactiveCommand
            .CreateFromObservable(ExecuteTextCommand);
    }

    private IObservable<Unit> ExecuteTextCommand()
    {
        TheText = "Hello ReactiveUI";
        return Observable.Return(Unit.Default);
    }
}
