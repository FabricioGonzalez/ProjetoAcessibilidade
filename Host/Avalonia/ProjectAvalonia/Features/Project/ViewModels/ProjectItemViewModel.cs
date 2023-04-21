using System.Reactive;
using ProjectAvalonia.Common.ViewModels;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

public abstract partial class ProjectItemViewModel : ViewModelBase
{
    [AutoNotify] private bool _inEditMode;

    [AutoNotify] private bool _isInEditMode;
    [AutoNotify] private string _path;
    [AutoNotify] private string _referencedItem;
    [AutoNotify] private string _title;

    public ProjectItemViewModel(
        string Title
        , string Path
        , string referencedItem
        , bool inEditMode
    )
    {
        this.Title = Title;
        this.Path = Path;
        InEditMode = inEditMode;
        ReferencedItem = referencedItem;

        RenameCommand = ReactiveCommand.Create(execute: () =>
        {
            InEditMode = true;
        });
        CommitChangeCommand = ReactiveCommand.Create(execute: () =>
        {
            LogHost.Default.Warn<ProjectItemViewModel>(message: "This needs to be Implemented to do something");
        });
    }

    public ReactiveCommand<Unit, Unit> RenameCommand
    {
        get;
        protected set;
    }

    public ReactiveCommand<Unit, Unit> CommitChangeCommand
    {
        get;
        protected set;
    }

    public ReactiveCommand<Unit, Unit> DeleteCommand
    {
        get;
        protected set;
    }
}