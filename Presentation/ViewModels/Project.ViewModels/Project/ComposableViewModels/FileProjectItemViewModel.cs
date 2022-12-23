using AppViewModels.Interactions.Project;

using ReactiveUI;

namespace AppViewModels.Project.ComposableViewModels;
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
            ProjectInteractions
                 .RenameFileInteraction
                 .Handle(this)
                 .Subscribe();
        });

        DeleteCommand = ReactiveCommand.Create(() =>
        {

        });

        DeleteCommand.Subscribe(disposable =>
        {
            ProjectInteractions
            .DeleteFileInteraction
            .Handle(this)
            .Subscribe();
        });
    }
}
