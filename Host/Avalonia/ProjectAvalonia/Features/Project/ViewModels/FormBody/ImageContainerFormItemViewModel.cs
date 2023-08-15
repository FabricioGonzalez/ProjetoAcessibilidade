using System.Collections.ObjectModel;
using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ImageContainerFormItemViewModel
    : ReactiveObject
        , IImageFormItemViewModel
{
    public ImageContainerFormItemViewModel(
        ObservableCollection<IImageItemViewModel> imageItems
        , string topic = "Imagens"
    )
    {
        ImageItems = imageItems;
        Topic = topic;

        AddPhotoCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            /*Option<string>.Some(await FileDialogHelper.ShowOpenFileDialogAsync("Get Images"))
                .Map(item =>
                {
                    return new ImageViewModel(item, "", Guid.NewGuid().ToString());
                })
                .Match(item =>
                {
                    ImageItems.Add(item);

                    return Empty.Value;
                }, () => Empty.Value);*/
        });

        RemoveImageCommand = ReactiveCommand.Create<IImageItemViewModel>(image =>
        {
            _ = ImageItems.Remove(image);
        });
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

    public ReactiveCommand<IImageItemViewModel, Unit> RemoveImageCommand
    {
        get;
    }
}