using System.Linq;
using DynamicData.Binding;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.FormItemState;
using ProjectAvalonia.Presentation.States.LawItemState;
using XmlDatasource.ProjectItems.DTO;
using XmlDatasource.ProjectItems.DTO.FormItem;

namespace ProjectAvalonia.Features.Project.Services;

public static class AppModelStateExtensions
{
    public static AppModelState ToAppModelState(
        this ItemRoot itemRoot
    )
    {
        var toReturn = new AppModelState
        {
            Id = itemRoot.Id, ItemName = itemRoot.ItemName, ItemTemplate = itemRoot.TemplateName
        };
        toReturn.LoadItemData(
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
        return toReturn;
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