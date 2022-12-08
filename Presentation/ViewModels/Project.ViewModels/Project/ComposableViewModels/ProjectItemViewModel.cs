using System.ComponentModel;
using System.Reactive;

using AppViewModels.Common;

using ReactiveUI;

namespace AppViewModels.Project.ComposableViewModels;
public abstract class ProjectItemViewModel : ViewModelBase
{
    public ProjectItemViewModel()
    {
        RenameCommand = ReactiveCommand.Create(() =>
        {
            InEditMode = true;
        });
    }

    private bool _isInEditMode;

    public string title;
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

    public ReactiveCommand<Unit, Unit> RenameCommand
    {
        get; protected set;
    }
    public ReactiveCommand<Unit, Unit> DeleteCommand
    {
        get; protected set;
    }
}
