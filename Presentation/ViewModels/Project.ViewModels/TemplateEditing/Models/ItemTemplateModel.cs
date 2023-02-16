using ReactiveUI;

namespace AppViewModels.TemplateEditing.Models;
public class ItemTemplateModel : ReactiveObject
{
    private string name = "";
    public string Name
    {
        get => name;
        set => this.RaiseAndSetIfChanged(ref name, value);
    }

    private string path = "";
    public string Path
    {
        get => path;
        set => this.RaiseAndSetIfChanged(ref path, value);
    }

    private bool isEditing = false;
    public bool IsEditing
    {
        get => isEditing;
        set => this.RaiseAndSetIfChanged(ref isEditing, value);
    }

}
