using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ImageViewModel : ReactiveObject, IImageItemViewModel
{
    public ImageViewModel(string imagePath, string imageObservation)
    {
        ImagePath = imagePath;
        ImageObservation = imageObservation;
    }

    public string ImagePath
    {
        get;
    }

    public string ImageObservation
    {
        get;
    }
}