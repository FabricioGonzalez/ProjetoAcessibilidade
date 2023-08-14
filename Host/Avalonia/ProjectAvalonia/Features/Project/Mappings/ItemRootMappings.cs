using System.Collections.Generic;
using System.Linq;
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
                .Where(it => it.Body is not ImageContainerItemState && it.Body is not ObservationItemState)
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
            , Images = new List<ImageItem>(), Observations = new List<ObservationModel>()
        };

        return toReturn;
        /*toReturn.LoadItemData(
            itemRoot
                ?.FormData
                ?.Select(
                    item =>
                    {
                        if (item is ItemFormDataCheckboxModel checkbox)
                        {
                            return new FormItemContainer
                            {
                                Topic = checkbox.Topic, Body = new CheckboxContainerItemState(checkbox.Topic)
                                {
                                    Id = checkbox.Id, Children = new ObservableCollectionExtended<CheckboxItemState>(
                                        checkbox.Children
                                            .Select(
                                                checkboxItem =>
                                                {
                                                    return new CheckboxItemState
                                                    {
                                                        Id = checkboxItem.Id, Topic = checkboxItem.Topic, TextItems =
                                                            new ObservableCollectionExtended<TextItemState>(
                                                                checkboxItem.TextItems.Select(
                                                                    textItem =>
                                                                    {
                                                                        return new TextItemState(
                                                                            id: textItem.Id,
                                                                            topic: textItem.Topic,
                                                                            textData: textItem.TextData,
                                                                            measurementUnit: textItem.MeasurementUnit);
                                                                    }))
                                                        , Options = new ObservableCollectionExtended<OptionsItemState>(
                                                            checkboxItem.Options
                                                                .Select(
                                                                    opt => new OptionsItemState
                                                                    {
                                                                        Id = opt.Id, IsChecked = opt.IsChecked
                                                                        , Value = opt.Value
                                                                    }))
                                                    };
                                                }))
                                }
                            };
                        }

                        if (item is ItemFormDataTextModel text)
                        {
                            return new FormItemContainer
                            {
                                Topic = text.Topic, Body = new TextItemState(
                                    id: text.Id,
                                    topic: text.Topic,
                                    textData: text.TextData,
                                    measurementUnit: text.MeasurementUnit)
                            };
                        }

                        return null;
                    }) ?? Enumerable.Empty<FormItemContainer>());

        toReturn.LoadLawItems(
            itemRoot
                .LawList
                .Select(
                    law =>
                        new LawStateItem { LawId = law.LawId, LawContent = law.LawTextContent }));
        return toReturn;*/
        /*return new AppModelState()
        {
            Id = itemRoot.Id,
            FormData = ,
            ItemName = itemRoot.ItemName,
            ItemTemplate = itemRoot.TemplateName,
            LawItems =
        }*/
    }
}