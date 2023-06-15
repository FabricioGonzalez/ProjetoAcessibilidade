using System.Reactive;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeEditableItemViewModel
    : ReactiveObject
        , IEditableItemViewModel
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
    } = "Teste";

    public string Name
    {
        get;
        set;
    } = "Teste";

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