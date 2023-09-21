using System.ComponentModel;
using System.Reactive;

using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IEditingItemViewModel : INotifyPropertyChanged, IDisposable
{
    public string ItemName
    {
        get; set;
    }

    public string DisplayName
    {
        get;
    }

    public string TemplateName
    {
        get;
    }

    public string Id
    {
        get;
    }

    public bool IsSaved
    {
        get;
    }

    public string ItemPath
    {
        get; set;
    }

    public IEditingBody Body
    {
        get;
        set;
    }

    public ReactiveCommand<Unit, Unit> CloseItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }
}