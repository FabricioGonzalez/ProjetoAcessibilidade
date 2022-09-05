using System.Xml;
using System.Text.RegularExpressions;

using SystemApplication.Services.Contracts;
using Core.Contracts;
using Core.Enums;
using SystemApplication.Services.UIOutputs;

namespace Infrastructure.InMemoryRepository;
public class XmlProjectDataRepository : IXmlProjectDataRepository
{
    public XmlProjectDataRepository()
    {

    }
    public async Task<ItemModel> GetModel(string path)
    {
        var task = new Task<ItemModel>(() => GetXMLData(path));

        task.Start();

        return await task;
    }

    private ItemModel GetXMLData(string path)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            using (StreamReader reader = new StreamReader(path))

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

    public async Task SaveModel(ItemModel model, string path)
    {
        await Task.Run(() =>
           {
               XmlDocument doc = new XmlDocument();

               var root = doc.CreateElement("item");

               doc.AppendChild(root);

               var itemName = doc.CreateElement("name");
               itemName.InnerXml = model.ItemName;

               root.AppendChild(itemName);

               var tabelaName = doc.CreateElement("tabela");

               foreach (var projectItem in model.FormData)
               {
                   var attribute = doc.CreateAttribute("item");
                   attribute.Value = $"{model.FormData.IndexOf(projectItem) + 1}";

                   var itemTabelaName = doc.CreateElement("itemtabela");

                   itemTabelaName.Attributes.Append(attribute);

                   if (projectItem.Type.Equals(FormDataItemTypeEnum.Checkbox))
                   {
                       var itemType = doc.CreateElement("tipo");
                       itemType.InnerXml = projectItem.Type.ToString()
                       .Replace(projectItem.Type.ToString()[0],
                       char.ToLower(projectItem.Type.ToString()[0]));

                       itemTabelaName.AppendChild(itemType);

                       XmlElement? checkboxes = doc.CreateElement("checkboxes");

                       foreach (var checkbox in (projectItem as FormDataItemCheckboxModel)!.Children)
                       {
                           var checkboxItemAttribute = doc.CreateAttribute("item");
                           attribute.Value = $"{(projectItem as FormDataItemCheckboxModel)!.Children.IndexOf(checkbox) + 1}";

                           var itemCheckbox = doc.CreateElement("checkboxitem");
                           itemCheckbox.Attributes.Append(attribute);

                           var checkboxItemTopic = doc.CreateElement("topico");
                           checkboxItemTopic.InnerXml = checkbox.Topic;

                           itemCheckbox.AppendChild(checkboxItemTopic);

                           XmlElement? options = doc.CreateElement("opcoes");

                           foreach (var option in checkbox.Options)
                           {
                               var itemOption = doc.CreateElement("opcao");

                               XmlAttribute? optionItemAttribute = doc.CreateAttribute("item");
                               attribute.Value = $"{checkbox.Options.IndexOf(option) + 1}";

                               itemOption.Attributes.Append(attribute);

                               var itemOptionValue = doc.CreateElement("value");
                               itemOptionValue.InnerText = option.Value;

                               var itemOptionChecked = doc.CreateElement("checked");
                               itemOptionChecked.InnerXml = option.IsChecked.ToString();

                               itemOption.AppendChild(itemOptionChecked);
                               itemOption.AppendChild(itemOptionValue);

                               options.AppendChild(itemOption);

                           }

                           itemCheckbox.AppendChild(options);

                           if (checkbox.TextItems is not null && checkbox.TextItems.Count > 0)
                               foreach (var textitem in checkbox.TextItems)
                               {
                                   var itemTexto = doc.CreateElement("texto");

                                   var itemTextoTopic = doc.CreateElement("topico");
                                   checkboxItemTopic.InnerXml = textitem.Topic;

                                   var itemTextoUnidade = doc.CreateElement("unidade");
                                   checkboxItemTopic.InnerXml = textitem.MeasurementUnit;

                                   var itemTextoValue = doc.CreateElement("value");
                                   checkboxItemTopic.InnerXml = textitem.TextData;

                                   itemTexto.AppendChild(itemTextoTopic);
                                   itemTexto.AppendChild(itemTextoUnidade);
                                   itemTexto.AppendChild(itemTextoValue);

                                   itemCheckbox.AppendChild(itemTexto);
                               }

                           checkboxes.AppendChild(itemCheckbox);
                       }

                       itemTabelaName.AppendChild(checkboxes);

                   }

                   if (projectItem.Type.Equals(FormDataItemTypeEnum.Observation))
                   {
                       var itemType = doc.CreateElement("tipo");
                       itemType.InnerXml = projectItem.Type.ToString()
                       .Replace(projectItem.Type.ToString()[0],
                       char.ToLower(projectItem.Type.ToString()[0]));


                       var itemObservationText = doc.CreateElement("value");
                       itemObservationText.InnerXml = (projectItem as FormDataItemObservationModel)!.Observation;

                       itemTabelaName.AppendChild(itemType);
                       itemTabelaName.AppendChild(itemObservationText);
                   }

                   if (projectItem.Type.Equals(FormDataItemTypeEnum.Text))
                   {
                       var itemType = doc.CreateElement("tipo");
                       itemType.InnerXml = projectItem.Type.ToString()
                       .Replace(projectItem.Type.ToString()[0],
                       char.ToLower(projectItem.Type.ToString()[0]));

                       var itemTextoTopic = doc.CreateElement("topico");
                       itemTextoTopic.InnerXml = projectItem.Topic;

                       var itemTextoUnidade = doc.CreateElement("unidade");
                       itemTextoUnidade.InnerXml = (projectItem as FormDataItemTextModel)!.MeasurementUnit!;

                       var itemTextoValue = doc.CreateElement("value");
                       itemTextoValue.InnerXml = (projectItem as FormDataItemTextModel)!.TextData;

                       itemTabelaName.AppendChild(itemType);
                       itemTabelaName.AppendChild(itemTextoTopic);
                       itemTabelaName.AppendChild(itemTextoUnidade);
                       itemTabelaName.AppendChild(itemTextoValue);
                   }

                   if (projectItem.Type.Equals(FormDataItemTypeEnum.Images))
                   {
                       var itemType = doc.CreateElement("tipo");
                       itemType.InnerXml = projectItem.Type.ToString()
                       .Replace(projectItem.Type.ToString()[0],
                       char.ToLower(projectItem.Type.ToString()[0]));

                       var itemImageImages = doc.CreateElement("images");

                       if ((projectItem as FormDataItemImageModel)!.Images.Count > 0)

                           foreach (var item in (projectItem as FormDataItemImageModel)!.Images)
                           {
                               var itemImageImagesPath = doc.CreateElement("image");
                               itemImageImagesPath.InnerXml = item;

                               itemImageImages.AppendChild(itemImageImagesPath);
                           }

                       itemTabelaName.AppendChild(itemType);
                       itemTabelaName.AppendChild(itemImageImages);
                   }

                   tabelaName.AppendChild(itemTabelaName);
               }

               root.AppendChild(tabelaName);

               var leiName = doc.CreateElement("lei");

               foreach (var item in model.LawList)
               {
                   var itemLei = doc.CreateElement("leiitem");

                   var itemLeiId = doc.CreateElement("id");
                   itemLeiId.InnerXml = item.LawId;

                   itemLei.AppendChild(itemLeiId);

                    var LeiDescricao = doc.CreateElement("descricao");     

                   foreach (var itemLeiDes in item.LawTextContent)
                   {
                       var LeiItemDescricao = doc.CreateElement("itemdescricao");
                       LeiItemDescricao.InnerXml = itemLeiDes;

                       LeiDescricao.AppendChild(LeiItemDescricao);
                   }

                   itemLei.AppendChild(LeiDescricao);

                   leiName.AppendChild(itemLei);
               }

               root.AppendChild(leiName);

               using (StreamWriter writer = new StreamWriter(path))
               {
                   writer.Flush();
                   doc.Save(writer);
               }

               doc = null;
               root = null;
               itemName = null;
               tabelaName = null;
           });
    }
}
