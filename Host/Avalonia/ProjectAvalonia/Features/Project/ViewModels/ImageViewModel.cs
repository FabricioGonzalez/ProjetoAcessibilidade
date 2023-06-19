using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ImageViewModel : ReactiveObject, IImageItemViewModel
{
    public ImageViewModel(string imagePath, string imageObservation, string id)
    {
        ImagePath = imagePath;
        ImageObservation = imageObservation;
        Id = id;
    }

    public string ImagePath
    {
        get; set;
    }


    public string ImageObservation
    {
        get;
    }
    public string Id
    {
        get;
        set;
    }
}