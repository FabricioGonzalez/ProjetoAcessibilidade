using ReactiveUI;

namespace AppViewModels.Interactions.Project;
public static class ProjectInteractions
{
    public static readonly Interaction<string, string> SelectedProjectPath = new();

}
