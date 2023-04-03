﻿using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.DTO.FormItem;
public class ItemFormDataCheckboxModel : ItemFormDataContainer
{
    public ItemFormDataCheckboxModel() : base()
    {

    }
    public ItemFormDataCheckboxModel(string id,
        string topic,
        ItemFormDataEnum type = ItemFormDataEnum.Checkbox)
        : base(id: id, topic: topic, type: type)
    {
        Children = new();
    }
    [XmlArray(elementName: "checkboxes")]
    [XmlArrayItem(elementName: "checkbox_item")]
    public List<ItemFormDataCheckboxChildModel> Children
    {
        get; set;
    }
}
