using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemApplication.Services.Contracts;

namespace SystemApplication.Services.ProjectDataServices;
public class GetProjectData
{
    IXmlProjectDataRepository repository;
    public GetProjectData(IXmlProjectDataRepository repository)
    {
        this.repository = repository;
    }
    public UIOutputs.ItemModel GetItemProject(string path)
    {
        var result = repository.GetModel(path);

        var item = new UIOutputs.ItemModel();

        item.ItemName = result.ItemName;
        item.FormData = new();

        foreach (var itemTable in result.FormData)
        {
            if (itemTable.Type == Core.Enums.FormDataItemTypeEnum.Checkbox)
            {
                var newitem = new UIOutputs.FormDataItemCheckboxModel()
                {
                    Topic = itemTable.Topic,
                    Children = new(),
                    Type = itemTable.Type
                };
                var itens = (itemTable as Core.Models.FormDataItemCheckboxModel).Children;

                itens.ForEach((e) =>
                {
                    var child = new UIOutputs.FormDataItemCheckboxChildModel()
                    {
                        Options = new(),
                        TextItems = new()
                    };

                    child.Topic = e.Topic;

                    e.Options.ForEach(i =>
                    {
                        var itemOption = new UIOutputs.OptionModel();
                        itemOption.Value = i.Value;
                        itemOption.IsChecked = i.IsChecked;

                        child.Options.Add(itemOption);
                    });

                    e.TextItems.ForEach(i =>
                    {
                        var textItem = new UIOutputs.FormDataItemTextModel();

                        textItem.TextData = i.TextData;
                        textItem.MeasurementUnit = i.MeasurementUnit;
                        textItem.Topic = i.Topic;
                        textItem.Type = i.Type;

                        child.TextItems.Add(textItem);

                    });

                    newitem.Children.Add(child);
                });

                item.FormData.Add(newitem);
            }

            if (itemTable.Type == Core.Enums.FormDataItemTypeEnum.Text)
            {
                var newTextItem = new UIOutputs.FormDataItemTextModel();

                newTextItem.Topic = itemTable.Topic;
                newTextItem.TextData = (itemTable as Core.Models.FormDataItemTextModel).TextData;
                newTextItem.MeasurementUnit = (itemTable as Core.Models.FormDataItemTextModel).MeasurementUnit;
                newTextItem.Type = itemTable.Type;

                item.FormData.Add(newTextItem);
            }

            if (itemTable.Type == Core.Enums.FormDataItemTypeEnum.Observation)
            {
                var newObservationItem = new UIOutputs.FormDataItemObservationModel();

                newObservationItem.Topic = itemTable.Topic;
                newObservationItem.Observation = (itemTable as Core.Models.FormDataItemObservationModel).Observation;
                newObservationItem.Type = itemTable.Type;

                item.FormData.Add(newObservationItem);
            }
        }

        foreach (var law in result.LawList)
        {
            item.LawList.Add(new()
            {
                LawId = law.LawId,
                LawTextContent = law.LawTextContent,
            });
        }

        return item;
    }
}
