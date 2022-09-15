using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using AppUsecases.Contracts.Repositories;
using AppUsecases.Entities;
using AppUsecases.Entities.AppFormDataItems.Checkbox;

namespace LocalRepository.FileRepository.Repository.XmlFile;
public class WriteXmlFileRepository : IWriteContract<AppItemModel>
{
    public async Task WriteFileAsync(AppItemModel dataToWrite, string filePathToWrite)
    {
        await Task.Run(() =>
        {
            XmlDocument doc = new XmlDocument();

            var root = doc.CreateElement("item");

            doc.AppendChild(root);

            var itemName = doc.CreateElement("name");
            itemName.InnerXml = dataToWrite.ItemName;

            root.AppendChild(itemName);

            var tabelaName = doc.CreateElement("tabela");

            foreach (var projectItem in dataToWrite.FormData)
            {
                var attribute = doc.CreateAttribute("item");
                attribute.Value = $"{dataToWrite.FormData.IndexOf(projectItem) + 1}";

                var itemTabelaName = doc.CreateElement("itemtabela");

                itemTabelaName.Attributes.Append(attribute);

                if (projectItem.Type.Equals(AppFormDataTypeEnum.Checkbox))
                {
                    var itemType = doc.CreateElement("tipo");
                    itemType.InnerXml = projectItem.Type.ToString()
                    .Replace(projectItem.Type.ToString()[0],
                    char.ToLower(projectItem.Type.ToString()[0]));

                    itemTabelaName.AppendChild(itemType);

                    XmlElement? checkboxes = doc.CreateElement("checkboxes");

                    foreach (var checkbox in (projectItem as AppFormDataItemCheckboxModel)!.Children)
                    {
                        var checkboxItemAttribute = doc.CreateAttribute("item");
                        attribute.Value = $"{(projectItem as AppFormDataItemCheckboxChildModel)!.Children.IndexOf(checkbox) + 1}";

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

