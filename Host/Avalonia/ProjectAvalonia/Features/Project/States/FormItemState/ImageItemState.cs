using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.FormItemState;
public class ImageItemState : ReactiveObject
{
    private string imagePath = "";
    public string ImagePath
    {
        get => imagePath;
        set => this.RaiseAndSetIfChanged(ref imagePath, value);
    }

    private string imageObservation = "";
    public string ImageObservation
    {
        get => imageObservation;
        set => this.RaiseAndSetIfChanged(ref imageObservation, value);
    }
}
