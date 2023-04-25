using System.Collections.ObjectModel;
using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ImageContainerFormItemViewModel : ReactiveObject, IImageFormItemViewModel
{
    public ImageContainerFormItemViewModel(ObservableCollection<IImageItemViewModel> imageItems, string topic)
    {
        ImageItems = imageItems;
        Topic = topic;
    }

    public ObservableCollection<IImageItemViewModel> ImageItems
    {
        get;
    }

    public string Topic
    {
        get;
    }


    public ReactiveCommand<Unit, Unit> AddPhotoCommand
    {
        get;
    }
}