using System.Linq;
using ProjectAvalonia.Features.Project.ViewModels;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.FormItemState;
using XmlDatasource.ProjectItems.DTO;
using XmlDatasource.ProjectItems.DTO.FormItem;

namespace ProjectAvalonia.Features.Project.Services;

public static class ItemRootMappings
{
    public static ItemRoot ToItemRoot(
        this AppModelState state
    )
    {
        var toReturn = new ItemRoot
        {
            Id = state.Id, ItemName = state.ItemName, TemplateName = state.ItemTemplate, FormData = state
                .FormData
                .Select<FormItemContainer, ItemFormDataContainer>(it =>
                {
                    if (it.Body is CheckboxContainerItemState checkbox)
                    {
                        return new ItemFormDataCheckboxModel(id: checkbox.Id, topic: checkbox.Topic)
                        {
                            Children = checkbox.Children.Select(child =>
                                new ItemFormDataCheckboxChildModel(id: child.Id, topic: child.Topic, isInvalid: false)
                                {
                                    TextItems = child.TextItems.Select(
                                            innerText => new ItemFormDataTextModel(id: innerText.Id
                                                , topic: innerText.Topic
                                                , textData: innerText.TextData
                                                , measurementUnit: innerText.MeasurementUnit))
                                        .ToList()
                                    , Options = child.Options.Select(option =>
                                        new ItemOptionModel(value: option.Value, id: option.Id
                                            , isChecked: option.IsChecked)
                                        {
                                            Id = option.Id, Value = option.Value, IsChecked = option.IsChecked
                                            , IsInvalid = option.IsInvalid
                                        }).ToList()
                                }).ToList()
                        };
                    }

                    if (it.Body is TextItemState text)
                    {
                        return new ItemFormDataTextModel(id: text.Id, topic: text.Topic
                            , textData: text.TextData, measurementUnit: text.MeasurementUnit);
                    }

                    return new ItemFormDataEmpty();
                }).ToList()
                .ToList()
            , LawList = state.LawItems.Select(law => new ItemLaw(lawId: law.LawId, lawContent: law.LawContent)).ToList()
            , Images = state.ImageContainer.ImagesItems.Select(it =>
                new ImageItem(id: it.Id, imagePath: it.ImagePath, imageObservation: it.ImageObservation)).ToList()
            , Observations = state.ObservationContainer.Observations
                .Select(it => new ObservationModel(observation: it.Observation, id: it.Id)).ToList()
        };

        return toReturn;
    }

    public static ItemRoot ToItemRoot(
        this IEditingItemViewModel state
    ) =>
        ((IEditingBodyViewModel)state.Body).ToAppModel(id: state.Id, itemName: state.ItemName
            , templateName: state.TemplateName);
}