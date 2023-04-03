﻿using System.Xml.Serialization;

namespace ProjectItemReader.XmlFile.DTO.FormItem;

public class ItemOptionModel : ItemFormData
{
    public ItemOptionModel() : base()
    {

    }
    public ItemOptionModel(string value, string id, bool isChecked = false)
        : base(id: id)
    {
        Id = id;
        Value = value;
        IsChecked = isChecked;
    }
    [XmlElement(elementName: "value")]
    public string Value
    {
        get; set;
    }
    [XmlElement(elementName: "is_checked")]
    public bool IsChecked
    {
        get; set;
    }
}