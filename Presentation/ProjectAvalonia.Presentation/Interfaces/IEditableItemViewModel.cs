using System.ComponentModel;
using System.Reactive;

using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IEditableItemViewModel : INotifyPropertyChanged
{
    public bool InEditMode
    {
        get;
        set;
    }

    public string TemplateName
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public string ItemPath
    {
        get;
        set;
    }

    public string Id
    {
        get;
        set;
    }

    public ReactiveCommand<IEditableItemViewModel, Unit> CommitItemCommand
    {
        get;init;
    }

    public ReactiveCommand<Unit, Unit> ExcludeItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> RenameItemCommand
    {
        get;
    }
}