using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;
public class FileProjectItemViewModel : ProjectItemViewModel
{
    public FileProjectItemViewModel(string title,
                                    string path,
                                    string referencedItem,
                                    bool inEditMode = false)
        : base(title,
            path,
            referencedItem,
            inEditMode)
    {
        RenameCommand = ReactiveCommand.Create(() =>
        {
            InEditMode = true;
        });

        CommitChangeCommand = ReactiveCommand.Create(() =>
        {

        });

        DeleteCommand = ReactiveCommand.Create(() =>
        {

        });
    }
}
