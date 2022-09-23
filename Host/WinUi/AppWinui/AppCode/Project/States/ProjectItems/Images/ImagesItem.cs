using ReactiveUI;

namespace AppWinui.AppCode.Project.States.ProjectItems.Images;

public class ImagesItem : ReactiveObject
{
    private string _imagePath = "";
    public string ImagePath
    {
        get => _imagePath;
        set => this.RaiseAndSetIfChanged(
            ref _imagePath,
            value,
            nameof(ImagePath));
    }
    private string _imageObservation = "";
    public string ImageObservation
    {
        get => _imageObservation; 
        set => this.RaiseAndSetIfChanged(
            ref _imageObservation,
            value,
            nameof(ImageObservation));
    }
}