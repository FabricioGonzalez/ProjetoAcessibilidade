using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using AppViewModels.Common;
using AppViewModels.Contracts;
using AppViewModels.Project.ComposableViewModels;
using AppViewModels.Project.States.ProjectItemState;
using AppViewModels.Project.States.ProjectItemState.FormItemState;
using AppViewModels.Project.States.ProjectItemState.LawItemState;

using Common;

using Core.Entities.Solution.Project.AppItem;
using Core.Entities.Solution.Project.AppItem.DataItems;
using Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using Core.Entities.Solution.Project.AppItem.DataItems.Images;
using Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using Core.Entities.Solution.Project.AppItem.DataItems.Text;
using Core.Enuns;

using DynamicData.Binding;

using Project.Domain.App.Models;
using Project.Domain.Contracts;
using Project.Domain.Project.Commands.ProjectItemCommands.SaveCommands;
using Project.Domain.Project.Queries.GetProjectItemContent;

using ReactiveUI;

using Splat;

namespace AppViewModels.Project;
public class ProjectItemEditingViewModel : ViewModelBase
{
    private AppModelState item;
    public AppModelState Item
    {
        get => item;
        set => this
            .RaiseAndSetIfChanged(
            ref item,
            value,
            nameof(Item));
    }

    private FileProjectItemViewModel selectedItem;
    public FileProjectItemViewModel SelectedItem
    {
        get => selectedItem;
        set => this
            .RaiseAndSetIfChanged(
            ref selectedItem,
            value,
            nameof(SelectedItem));
    }

    private readonly IQueryDispatcher? queryDispatcher;
    private readonly ICommandDispatcher? commandDispatcher;
    private readonly IFileDialog? fileSelector;

    public ProjectItemEditingViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();
        fileSelector ??= Locator.Current.GetService<IFileDialog>();

        var canSave = this
           .WhenAnyValue(vm => vm.Item)
           .Select(prop => prop is not null);

        SaveItemCommand = ReactiveCommand.CreateFromTask<AppModelState>(
       execute: async (appModel) =>
       {
           if (appModel is not null)
           {
               AppItemModel itemModel = new()
               {
                   Id = appModel.Id,
                   TemplateName = appModel.TemplateName,
                   ItemName = appModel.ItemName,
                   FormData = appModel.FormData.Select<ReactiveObject, IAppFormDataItemContract>(item =>
                   {
                       if (item is CheckboxContainerItemState checkbox)
                       {
                           return new AppFormDataItemCheckboxModel()
                           {
                               Topic = checkbox.Topic,
                               Type = checkbox.Type,
                               Children = checkbox.Children.Select(item => new AppFormDataItemCheckboxChildModel()
                               {
                                   Topic = item.Topic,
                                   Options = item
                                    .Options
                                    .Select(item =>
                                    new AppOptionModel()
                                    {
                                        IsChecked = item.IsChecked,
                                        Value = item.Value
                                    })
                                    .ToList(),
                                   TextItems =
                                   item
                                   .TextItems
                                   .Select(item => new AppFormDataItemTextModel()
                                   {
                                       Topic = item.Topic,
                                       MeasurementUnit = item.MeasurementUnit,
                                       TextData = item.TextData,
                                       Type = item.Type
                                   })
                                   .ToList()
                               })
                               .ToList(),
                           };
                       }
                       if (item is TextItemState text)
                       {
                           return new AppFormDataItemTextModel()
                           {
                               Topic = text.Topic,
                               MeasurementUnit = text.MeasurementUnit,
                               TextData = text.TextData,
                               Type = text.Type
                           };
                       }
                       if (item is ImageContainerItemState images)
                       {
                           return new AppFormDataItemImageModel()
                           {
                               Topic = images.Topic,
                               ImagesItems = images.ImagesItems.Select(item => new ImagesItem()
                               {
                                   imageObservation = item.ImageObservation,
                                   imagePath = item.ImagePath
                               }).ToList(),
                               Type = images.Type
                           };
                       }
                       if (item is ObservationItemState observation)
                       {
                           return new AppFormDataItemObservationModel()
                           {
                               Topic = observation.Topic,
                               Observation = observation.Observation,
                               Type = AppFormDataType.Observação
                           };
                       }
                       return null;

                   }).ToList(),
                   LawList = appModel
                    .LawItems
                    .Select(x => new AppLawModel()
                    {
                        LawId = x.LawId,
                        LawTextContent = x.LawContent
                    })
                    .ToList()
               };

               await commandDispatcher
                 ?.Dispatch<SaveProjectItemContentCommand, Resource<Empty>>(
                     command: new(
                         AppItem: itemModel, ItemPath: SelectedItem.Path),
                     cancellation: CancellationToken.None);



           }
       },
       canExecute: canSave);

        AddPhotoCommand = ReactiveCommand.Create<ImageContainerItemState>(
     execute: async (imageContainer) =>
     {
         var file = await fileSelector?.GetFile(fileFilters: new string[] { ".png", ".jpeg" });

         if (!string.IsNullOrWhiteSpace(file))
         {
             imageContainer.ImagesItems.Add(new() { ImagePath = file });
         }
     },
     canExecute: canSave);


        this.WhenActivated((CompositeDisposable disposables) =>
        {


        });
    }

    public ReactiveCommand<AppModelState, Unit> SaveItemCommand
    {
        get; private set;
    }
    public ReactiveCommand<ImageContainerItemState, Unit> AddPhotoCommand
    {
        get; private set;
    }
    public async Task SetEditingItem(string path)
    {
        var result = await queryDispatcher
            .Dispatch<GetProjectItemContentQuery, Resource<AppItemModel>>(
            new(path),
            CancellationToken.None);

        if (result is Resource<AppItemModel>.Error err) { }
        if (result is Resource<AppItemModel>.Success success)
        {
            Item = new()
            {
                ItemName = success?.Data?.ItemName ?? "",
                Id = success?.Data?.ItemName ?? Guid.NewGuid().ToString(),
                FormData = new ObservableCollectionExtended<ReactiveObject>(
                success
                        ?.Data
                        ?.FormData
                        ?.Select<IAppFormDataItemContract, ReactiveObject>(item =>
            {
                if (item is AppFormDataItemCheckboxModel checkbox)
                {
                    return new CheckboxContainerItemState()
                    {
                        Topic = checkbox.Topic,
                        Children = new(checkbox.Children
                        .Select(checkboxItem =>
                        {
                            return new CheckboxItemState()
                            {
                                Topic = checkboxItem.Topic,
                                TextItems = new(
                                    checkboxItem.TextItems.Select(textItem =>
                                    {
                                        return new TextItemState()
                                        {
                                            Topic = textItem.Topic,
                                            Type = textItem.Type,
                                            MeasurementUnit = textItem.MeasurementUnit ?? "",
                                            TextData = textItem.TextData,
                                        };
                                    })),
                                Options = new(
                                    checkboxItem.Options
                                    .Select(opt => new OptionsItemState() { IsChecked = opt.IsChecked, Value = opt.Value }))
                            };
                        }))
                    };
                }

                if (item is AppFormDataItemTextModel text)
                {
                    return new TextItemState()
                    {
                        Topic = text.Topic,
                        Type = text.Type,
                        MeasurementUnit = text?.MeasurementUnit ?? "",
                        TextData = text?.TextData ?? "",
                    };
                }

                if (item is AppFormDataItemObservationModel observation)
                {
                    return new ObservationItemState()
                    {
                        Topic = observation.Topic,
                        Observation = observation.Observation
                    };
                }

                if (item is AppFormDataItemImageModel images)
                {
                    return new ImageContainerItemState()
                    {
                        Topic = images.Topic,
                        Type = images.Type,
                        ImagesItems = new(
                            images
                        .ImagesItems
                        .Select(image =>
                        new ImageItemState()
                        {
                            ImagePath = image.imagePath,
                            ImageObservation = image.imageObservation
                        }))
                    };
                }
                return null;
            }) ?? Enumerable.Empty<ReactiveObject>()),
                LawItems = new ObservableCollectionExtended<LawStateItem>(
                    success
                ?.Data
                ?.LawList
                ?.Select(item =>
                new LawStateItem() { LawId = item.LawId, LawContent = item.LawTextContent }) ?? Enumerable.Empty<LawStateItem>()),
            };



        }
    }
}
