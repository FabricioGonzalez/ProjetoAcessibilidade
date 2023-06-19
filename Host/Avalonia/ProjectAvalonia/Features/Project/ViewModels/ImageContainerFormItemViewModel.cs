﻿using System;
using System.Collections.ObjectModel;
using System.Reactive;

using Common.Optional;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Presentation.Interfaces;

using ProjetoAcessibilidade.Domain.App.Models;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class ImageContainerFormItemViewModel : ReactiveObject, IImageFormItemViewModel
{
    public ImageContainerFormItemViewModel(ObservableCollection<IImageItemViewModel> imageItems, string topic)
    {
        ImageItems = imageItems;
        Topic = topic;

        AddPhotoCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            (await FileDialogHelper.ShowOpenFileDialogAsync("Get Images")).ToOption()
             .Map(item =>
             {
                 return new ImageViewModel(imagePath: item, "", Guid.NewGuid().ToString());
             })
             .MapValue(item =>
             {
                 ImageItems.Add(item);

                 return Empty.Value;
             })
             .Reduce(() => Empty.Value);
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
}