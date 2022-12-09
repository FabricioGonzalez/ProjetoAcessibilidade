using System.Collections.ObjectModel;

using AppViewModels.Interactions.Project;

using DynamicData.Binding;

using ReactiveUI;

namespace AppViewModels.Project.ComposableViewModels;
public class FolderProjectItemViewModel : ProjectItemViewModel
{
    public FolderProjectItemViewModel()
    {
        RenameCommand = ReactiveCommand.Create(() =>
        {
            InEditMode = true;
        });

        DeleteCommand = ReactiveCommand.Create(() =>
        {

        });

        this.WhenPropertyChanged(vm => vm.Title, false)
             .Subscribe(item =>
             {
                 if (item.Value != null)
                 {
                     ProjectInteractions
                    .RenameFolderInteraction
                    .Handle(this)
                    .Subscribe();
                 }
             });

        DeleteCommand.Subscribe(disposable =>
        {
            ProjectInteractions
            .DeleteFolderInteraction
            .Handle(this)
            .Subscribe();
        });

    }
    private ObservableCollection<ProjectItemViewModel> _children = new();
    public ObservableCollection<ProjectItemViewModel> Children
    {
        get => _children;
        set => this.RaiseAndSetIfChanged(ref _children, value);
    }
}
