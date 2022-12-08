using ReactiveUI;

namespace AppViewModels.Project.ComposableViewModels;
public class FileProjectItemViewModel : ProjectItemViewModel
{
    public FileProjectItemViewModel()
    {
        RenameCommand = ReactiveCommand.Create(() =>
        {
            InEditMode = true;
        });
    }

    /* private bool _isInEditMode;

     public  string Title
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
         get;
     }*/
}
