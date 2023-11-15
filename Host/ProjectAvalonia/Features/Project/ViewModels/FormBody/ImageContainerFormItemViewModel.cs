using System;
using System.Collections.ObjectModel;
using System.Reactive;

using Common.Linq;

using ProjectAvalonia.Common.Helpers;
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
            (await FileDialogHelper.GetMultipleImagesAsync())
                .IfSucc(succ =>
                {
                    succ.IterateOn(it =>
                    {
                        var image = new ImageViewModel(imagePath: it.Path.LocalPath, imageObservation: ""
                       , id: Guid.NewGuid().ToString());

                        ImageItems.Add(image);
                    });
                });
        });

        RemoveImageCommand = ReactiveCommand.Create<IImageItemViewModel>(image =>
        {
            ImageItems.Remove(image);
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