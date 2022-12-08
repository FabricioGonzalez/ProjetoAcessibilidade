using AppViewModels.Project.ComposableViewModels;

using ReactiveUI;

namespace AppViewModels.Interactions.Project;
public static class ProjectInteractions
{
    public static readonly Interaction<string, string> SelectedProjectPath = new();

    public static readonly Interaction<FileProjectItemViewModel, FileProjectItemViewModel> DeleteFileInteraction = new();
    public static readonly Interaction<FileProjectItemViewModel, FileProjectItemViewModel> RenameFileInteraction = new();
   
    public static readonly Interaction<FolderProjectItemViewModel, FolderProjectItemViewModel> DeleteFolderInteraction = new();
    public static readonly Interaction<FolderProjectItemViewModel, FolderProjectItemViewModel> RenameFolderInteraction = new();

}
