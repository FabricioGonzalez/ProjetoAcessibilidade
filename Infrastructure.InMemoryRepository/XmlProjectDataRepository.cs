using System.Xml;
using System.Text.RegularExpressions;

using Core.Contracts;
using Core.Enums;
using Core.Models;

using SystemApplication.Services.Contracts;

namespace Infrastructure.InMemoryRepository;
public class XmlProjectDataRepository : IXmlProjectDataRepository
{
    public ItemModel GetModel(string path)
    {
        try
        {
            XmlDocument doc = new XmlDocument();

            StreamReader reader = new StreamReader(path);

            doc.Load(reader);

            ItemModel model = new();

            var root = doc.GetElementsByTagName("item")[0];

            model.ItemName = root!.ChildNodes[0]!.InnerXml.Replace("\n", "");

            var modelTable = new List<IFormDataItemContract>();

            var lawModel = new List<LawModel>();

            foreach (var itemTable in root.ChildNodes[1]?.ChildNodes)
            {
                var res = (itemTable as XmlNode)?.ChildNodes[0]?.InnerXml;

                Enum.TryParse(typeof(FormDataItemTypeEnum),
                    Regex.Replace(res, "^[a-z]", m => m.Value.ToUpper()),
                    out var type);

                if ((FormDataItemTypeEnum)type! == FormDataItemTypeEnum.Text)
                {
                    var topicoText = (itemTable as XmlNode)!.ChildNodes[1]!.InnerXml.Replace("\n", "");
                    var unidadeText = "";
                    string value = "";

                    if ((itemTable as XmlNode)!.ChildNodes[2]!.Name == "value")
                    {
                        value = (itemTable as XmlNode)!.ChildNodes[2]!.InnerXml;
                    }
                    else
                    {
                        unidadeText = (itemTable as XmlNode)!.ChildNodes[2]!.InnerXml;
                        value = (itemTable as XmlNode)!.ChildNodes[3]!.InnerXml;
                    }

                    var textItem = new FormDataItemTextModel()
                    {
                        Type = (FormDataItemTypeEnum)type,
                        MeasurementUnit = unidadeText,
                        TextData = value,
                        Topic = topicoText
                    };
                    modelTable.Add(textItem);
                }

                if ((FormDataItemTypeEnum)type == FormDataItemTypeEnum.Checkbox)
                {
                    var checkboxItem = new FormDataItemCheckboxModel()
                    {
                        Type = (FormDataItemTypeEnum)type,
                        Children = new List<FormDataItemCheckboxChildModel>(),
                    };

                    XmlNodeList? checkboxes;

                    if ((itemTable as XmlNode)!.ChildNodes[1]!.Name == "topico")
                    {
                        checkboxItem.Topic = (itemTable as XmlNode)!.ChildNodes[1]!.InnerXml.Replace("\n", "");
                        checkboxes = (itemTable as XmlNode)!.ChildNodes[2]!.ChildNodes;
                    }
                    else
                    {
                        checkboxes = (itemTable as XmlNode)!.ChildNodes[1]!.ChildNodes;
                    }

                    foreach (XmlNode item in checkboxes)
                    {
                        var checkbox = item;

                        var newItem = new FormDataItemCheckboxChildModel();
                        newItem.Options = new List<OptionModel>();

                        newItem.Topic = checkbox.ChildNodes[0]!.InnerXml.Replace("\n", "");

                        foreach (XmlNode options in checkbox.ChildNodes[1]!.ChildNodes)
                        {
                            var optionItem = new OptionModel();

                            optionItem.IsChecked = bool.Parse(options.ChildNodes[0]!.InnerXml);
                            optionItem.Value = options.ChildNodes[1]!.InnerXml;

                            newItem.Options.Add(optionItem);
                        }
                        newItem.TextItems = new List<FormDataItemTextModel?>();

                        if (checkbox.ChildNodes[2] is not null && checkbox.ChildNodes[2].Name == "texto")
                        {
                            var textItem = new FormDataItemTextModel()
                            {
                                Topic = checkbox.ChildNodes[2].ChildNodes[0].InnerXml,
                                Type = FormDataItemTypeEnum.Text,
                                MeasurementUnit = checkbox.ChildNodes[2].ChildNodes[1].InnerXml,
                                TextData = checkbox.ChildNodes[2].ChildNodes[2].InnerXml
                            };

                            newItem.TextItems.Add(textItem);
                        }

                        checkboxItem.Children.Add(newItem);
                    }
                    modelTable.Add(checkboxItem);
                }

                if ((FormDataItemTypeEnum)type == FormDataItemTypeEnum.Observation)
                {
                    var value = (itemTable as XmlNode)!.ChildNodes[1]!.InnerXml;

                    var observation = new FormDataItemObservationModel()
                    {
                        Type = (FormDataItemTypeEnum)type,
                        Observation = value,
                    };
                    modelTable.Add(observation);
                }

            }

            foreach (XmlNode itemLaw in root.ChildNodes[2]!.ChildNodes)
            {
                var law = new LawModel();

                law.LawId = itemLaw.ChildNodes[0]?.InnerXml;
                law.LawTextContent = new List<string>();

                foreach (XmlNode item in itemLaw.ChildNodes[1].ChildNodes)
                {
                    law.LawTextContent.Add(item.InnerXml);
                }
                lawModel.Add(law);
            }

            model.FormData = modelTable;
            model.LawList = lawModel;

            return model;

        }
        catch (Exception)
        {
            throw;
        }


    }

}
