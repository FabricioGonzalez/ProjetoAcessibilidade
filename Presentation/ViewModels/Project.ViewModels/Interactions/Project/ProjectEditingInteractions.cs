using AppUsecases.Project.Entities.Project.EditingItems;

using AppViewModels.Project.ComposableViewModels;

using ReactiveUI;

namespace AppViewModels.Interactions.Project;
public static class ProjectEditingInteractions
{
    public static readonly Interaction<FileProjectItemViewModel, FileProjectItemViewModel> EditItem = new();
}
