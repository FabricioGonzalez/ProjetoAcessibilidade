using System.Reactive;

using AppViewModels.Common;

using ReactiveUI;

namespace AppViewModels.Project.ComposableViewModels;
public abstract class ProjectItemViewModel : ViewModelBase
{
    public ProjectItemViewModel(string Title, string Path, string referencedItem, bool inEditMode)
    {
        title = Title;
        this.Path = Path;
        InEditMode = inEditMode;
        ReferencedItem = referencedItem;

        RenameCommand = ReactiveCommand.Create(() =>
        {
            InEditMode = true;
        });
    }

    private bool _isInEditMode;

    private string title;
    public string Title
    {
        get => title;
        set => this.RaiseAndSetIfChanged(ref title, value, nameof(Title));
    }
    public string Path
    {
        get; set;
    }
    public bool InEditMode
    {
        get => _isInEditMode;
        set => this.RaiseAndSetIfChanged(ref _isInEditMode, value);
    }

    public string ReferencedItem
    {
        get; set;
    }

    public ReactiveCommand<Unit, Unit> RenameCommand
    {
        get; protected set;
    }
    public ReactiveCommand<Unit, Unit> DeleteCommand
    {
        get; protected set;
    }
}
