using System.Collections.ObjectModel;
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
        try
        {

        
        var toReturn = new AppModelState
        {
            Id = itemRoot.Id,
            ItemName = itemRoot.ItemName,
            ItemTemplate = itemRoot.TemplateName
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
                                Topic = checkbox.Topic,
                                Type = Presentation.Enums.AppFormDataType.Checkbox,
                                Body = new CheckboxContainerItemState(checkbox.Topic)
                                {
                                    Id = checkbox.Id,
                                    Children = new ObservableCollectionExtended<CheckboxItemState>(
                                        checkbox.Children
                                            .Select(
                                                checkboxItem =>
                                                {
                                                    return new CheckboxItemState
                                                    {
                                                        Id = checkboxItem.Id,
                                                        Topic = checkboxItem.Topic,
                                                        TextItems =
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
                                                        ,
                                                        Options = new ObservableCollectionExtended<OptionsItemState>(
                                                            checkboxItem.Options
                                                                .Select(
                                                                    opt => new OptionsItemState
                                                                    {
                                                                        Id = opt.Id,
                                                                        IsChecked = opt.IsChecked
                                                                        ,
                                                                        Value = opt.Value,
                                                                        IsInvalid = opt.IsInvalid
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
                                Topic = text.Topic,
                                Type = Presentation.Enums.AppFormDataType.Text,
                                Body = new TextItemState(
                                    id: text.Id,
                                    topic: text.Topic,
                                    textData: text.TextData,
                                    measurementUnit: text.MeasurementUnit)
                            };
                        }

                        return null;
                    }) ?? Enumerable.Empty<FormItemContainer>());
        toReturn.ImageContainer = new ImageContainerItemState
        {
            ImagesItems = new ObservableCollection<ImageItemState>(itemRoot?.Images.Select(it => new ImageItemState
            {
                Id = it.Id,
                ImageObservation = it.ImageObservation,
                ImagePath = it.ImagePath
            }) ?? Enumerable.Empty<ImageItemState>())
        };
        toReturn.ObservationContainer = new ObservationContainerItemState
        {
            Observations = new ObservableCollection<ObservationItemState>(
                itemRoot?.Observations.Select(it => new ObservationItemState(topic: "", observation: it.Observation)) ?? Enumerable.Empty<ObservationItemState>())
        };
        toReturn.LoadLawItems(
            itemRoot
                ?.LawList
                .Select(
                    law =>
                        new LawStateItem { LawId = law.LawId, LawContent = law.LawTextContent }) ?? Enumerable.Empty<LawStateItem>());
        return toReturn;
        }
        catch (System.Exception)
        {

            throw;
        }
    }
}