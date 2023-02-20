using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common;

using Core.Entities.Solution.ItemsGroup;
using Core.Entities.Solution.Project.AppItem;
using Core.Entities.Solution.Project.AppItem.DataItems;
using Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using Core.Entities.Solution.Project.AppItem.DataItems.Images;
using Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using Core.Entities.Solution.Project.AppItem.DataItems.Text;

using Project.Application.Contracts;
using Project.Application.Project.Queries.GetProjectItemContent;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.Project.States;
using ProjectAvalonia.Features.Project.States.FormItemState;
using ProjectAvalonia.Features.Project.States.LawItemState;
using ProjectAvalonia.ViewModels;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;
public partial class ProjectEditingViewModel : ViewModelBase
{
    [AutoNotify] private ItemModel _selectedItem;
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


    private readonly IQueryDispatcher? queryDispatcher;
    private readonly ICommandDispatcher? commandDispatcher;

    public ProjectEditingViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();

        var canSave = this
           .WhenAnyValue(vm => vm.Item)
           .Select(prop => prop is not null);

        /*  SaveItemCommand = ReactiveCommand.CreateFromTask<AppModelState>(
         execute: async (appModel) =>
         {
             if (appModel is not null)
             {
                 AppItemModel itemModel = new()
                 {
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
                                 Type = Core.Enuns.AppFormDataType.Observação
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
                   .Dispatch<SaveProjectItemContentCommand, Resource<object>>(
                       new(
                           itemModel, SelectedItem.ItemPath),
                       CancellationToken.None);

             }
         },
         canExecute: canSave);*/

        AddPhotoCommand = ReactiveCommand.Create<ImageContainerItemState>(
     execute: async (imageContainer) =>
     {
         var file = await FileDialogHelper.ShowOpenFileDialogAsync("Obter Image", filterExtTypes: new string[] { ".png", ".jpeg" });

         if (!string.IsNullOrWhiteSpace(file))
         {
             imageContainer.ImagesItems.Add(new() { ImagePath = file });
         }
     },
     canExecute: canSave);

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
                ItemName = success.Data.ItemName,
                FormData = new(
                    success
                .Data
                .FormData
                .Select<IAppFormDataItemContract, ReactiveObject>(item =>
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
                                                MeasurementUnit = textItem.MeasurementUnit,
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
                            MeasurementUnit = text.MeasurementUnit,
                            TextData = text.TextData,
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
                })),
                LawItems = new(
                    success
                .Data
                .LawList
                .Select(item =>
                new LawStateItem() { LawId = item.LawId, LawContent = item.LawTextContent }))
            };
        }
    }
}
