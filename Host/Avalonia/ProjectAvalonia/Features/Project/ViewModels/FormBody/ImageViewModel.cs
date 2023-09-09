using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public partial class ImageViewModel
    : ReactiveObject
        , IImageItemViewModel
{
    [AutoNotify]
    private string _imageObservation;

    public ImageViewModel(
        string imagePath
        , string imageObservation
        , string id
    )
    {
        ImagePath = imagePath;
        ImageObservation = imageObservation;
        Id = id;
    }

    public string ImagePath
    {
        get;
        set;
    }

    public string Id
    {
        get;
        set;
    }
}