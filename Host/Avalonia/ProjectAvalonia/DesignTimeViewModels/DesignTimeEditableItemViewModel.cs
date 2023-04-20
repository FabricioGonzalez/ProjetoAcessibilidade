using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeEditableItemViewModel : ReactiveObject, IEditableItemViewModel
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

    public ReactiveCommand<Unit, Unit> CommitItemCommand
    {
        get;
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