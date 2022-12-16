using AppViewModels.Interactions.Project;

using DynamicData.Binding;

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

        DeleteCommand = ReactiveCommand.Create(() =>
        {

        });

        this.WhenPropertyChanged(vm => vm.Title)
            .Subscribe(item =>
            {
                if (item.Value != null)
                {
                    ProjectInteractions
                    .RenameFileInteraction
                    .Handle(this)
                    .Subscribe();
                }
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
