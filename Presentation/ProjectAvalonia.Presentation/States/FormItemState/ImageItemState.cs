using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class ImageItemState
    : ReactiveObject
        , IIdentifiedItem
{
    private string _id = "";

    private string _imageObservation = "";

    private string _imagePath = "";

    public string ImageObservation
    {
        get => _imageObservation;
        set => this.RaiseAndSetIfChanged(ref _imageObservation, value);
    }

    public string ImagePath
    {
        get => _imagePath;
        set => this.RaiseAndSetIfChanged(ref _imagePath, value);
    }

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }
}