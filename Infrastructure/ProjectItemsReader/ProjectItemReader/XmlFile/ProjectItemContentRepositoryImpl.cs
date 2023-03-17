using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using Core.Entities.Solution.Project.AppItem;
using Core.Entities.Solution.Project.AppItem.DataItems;
using Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using Core.Entities.Solution.Project.AppItem.DataItems.Images;
using Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using Core.Entities.Solution.Project.AppItem.DataItems.Text;
using Core.Enuns;

using Project.Domain.Project.Contracts;

namespace ProjectItemReader.XmlFile;
public class ProjectItemContentRepositoryImpl : IProjectItemContentRepository
{
    public async Task<AppItemModel> GetSystemProjectItemContent(string filePathToWrite)
    {
        var task = new Task<AppItemModel>(() =>
        {
            try
            {
                var doc = new XmlDocument();
                using (var reader = new StreamReader(filePathToWrite))
                {
                    doc.Load(reader);
                }

                var model = new AppItemModel();

                var root = doc.GetElementsByTagName("item")[0];

                model.ItemName = root!.ChildNodes[0]!.InnerXml.Replace("\n", "");

                var modelTable = new ObservableCollection<IAppFormDataItemContract>();

                var lawModel = new List<AppLawModel>();

                foreach (var itemTable in root.ChildNodes[1]?.ChildNodes)
                {
                    var res = (itemTable as XmlNode)?.ChildNodes[0]?.InnerXml;

                    Enum.TryParse(typeof(AppFormDataType),
                        Regex.Replace(res, "^[a-z]", m => m.Value.ToUpper()),
                        out var type);

                    if ((AppFormDataType)type! == AppFormDataType.Texto)
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

                        var textItem = new AppFormDataItemTextModel()
                        {
                            Type = (AppFormDataType)type,
                            MeasurementUnit = unidadeText,
                            TextData = value,
                            Topic = topicoText
                        };
                        modelTable.Add(textItem);
                    }

                    if ((AppFormDataType)type == AppFormDataType.Checkbox)
                    {
                        var checkboxItem = new AppFormDataItemCheckboxModel()
                        {
                            Type = (AppFormDataType)type,
                            Children = new List<AppFormDataItemCheckboxChildModel>(),
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

                            var newItem = new AppFormDataItemCheckboxChildModel
                            {
                                Options = new ObservableCollection<AppOptionModel>(),

                                Topic = checkbox.ChildNodes[0]!.InnerXml.Replace("\n", "")
                            };

                            foreach (XmlNode options in checkbox.ChildNodes[1]!.ChildNodes)
                            {
                                var optionItem = new AppOptionModel
                                {
                                    IsChecked = bool.Parse(options.ChildNodes[0]!.InnerXml),
                                    Value = options.ChildNodes[1]!.InnerXml
                                };

                                newItem.Options.Add(optionItem);
                            }
                            newItem.TextItems = new ObservableCollection<AppFormDataItemTextModel?>();

                            if (checkbox.ChildNodes[2] is not null && checkbox.ChildNodes[2].Name == "texto")
                            {
                                var textItem = new AppFormDataItemTextModel()
                                {
                                    Topic = checkbox.ChildNodes[2].ChildNodes[0].InnerXml,
                                    Type = AppFormDataType.Texto,
                                    MeasurementUnit = checkbox.ChildNodes[2].ChildNodes[1].InnerXml,
                                    TextData = checkbox.ChildNodes[2].ChildNodes[2].InnerXml
                                };

                                newItem.TextItems.Add(textItem);
                            }

                            checkboxItem.Children.Add(newItem);
                        }
                        modelTable.Add(checkboxItem);
                    }

                    if ((AppFormDataType)type == AppFormDataType.Observação)
                    {
                        var value = (itemTable as XmlNode)!.ChildNodes[1]!.InnerXml;

                        var observation = new AppFormDataItemObservationModel()
                        {
                            Type = (AppFormDataType)type,
                            Observation = value,
                        };
                        modelTable.Add(observation);
                    }

                    if ((AppFormDataType)type == AppFormDataType.Image)
                    {
                        var value = (itemTable as XmlNode);

                        var image = new AppFormDataItemImageModel()
                        {
                            Type = (AppFormDataType)type,
                            ImagesItems = new List<ImagesItem>(),
                            Topic = "Imagens",
                        };

                        if ((itemTable as XmlNode)!.ChildNodes[1] is not null)
                        {
                            foreach (XmlNode item in (itemTable as XmlNode)!.ChildNodes[1])
                            {
                                image.ImagesItems.Add(new()
                                {
                                    imagePath = item?.ChildNodes[0]?.InnerXml,
                                    imageObservation = item?.ChildNodes[1]?.InnerXml,
                                });
                            }

                        }
                        modelTable.Add(image);
                    }
                }

                foreach (XmlNode itemLaw in root.ChildNodes[2]!.ChildNodes)
                {
                    var law = new AppLawModel
                    {
                        LawId = itemLaw.ChildNodes[0]?.InnerXml
                    };

                    var lawContent = new StringBuilder();

                    foreach (XmlNode item in itemLaw.ChildNodes[1].ChildNodes)
                    {
                        lawContent.AppendLine(item.InnerXml);
                    }

                    law.LawTextContent = lawContent.ToString();

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
        });

        task.Start();

        return await task;
    }
    public async Task<AppItemModel> GetProjectItemContent(string filePathToWrite)
    {
        var task = new Task<AppItemModel>(() =>
        {
            try
            {
                var doc = new XmlDocument();
                using (var reader = new StreamReader(filePathToWrite))
                {
                    doc.Load(reader);
                }

                var model = new AppItemModel();

                var root = doc.GetElementsByTagName("item")[0];

                model.Id = root!.ChildNodes[0]!.InnerXml.Replace("\n", "");
                model.ItemName = root!.ChildNodes[1]!.InnerXml.Replace("\n", "");
                model.TemplateName = root!.ChildNodes[2]!.InnerXml.Replace("\n", "");

                var modelTable = new ObservableCollection<IAppFormDataItemContract>();

                var lawModel = new List<AppLawModel>();

                foreach (var itemTable in root.ChildNodes[3]?.ChildNodes)
                {
                    var res = (itemTable as XmlNode)?.ChildNodes[0]?.InnerXml;

                    Enum.TryParse(typeof(AppFormDataType),
                        Regex.Replace(res, "^[a-z]", m => m.Value.ToUpper()),
                        out var type);

                    if ((AppFormDataType)type! == AppFormDataType.Texto)
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

                        var textItem = new AppFormDataItemTextModel()
                        {
                            Type = (AppFormDataType)type,
                            MeasurementUnit = unidadeText,
                            TextData = value,
                            Topic = topicoText
                        };
                        modelTable.Add(textItem);
                    }

                    if ((AppFormDataType)type == AppFormDataType.Checkbox)
                    {
                        var checkboxItem = new AppFormDataItemCheckboxModel()
                        {
                            Type = (AppFormDataType)type,
                            Children = new List<AppFormDataItemCheckboxChildModel>(),
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

                            var newItem = new AppFormDataItemCheckboxChildModel
                            {
                                Options = new ObservableCollection<AppOptionModel>(),

                                Topic = checkbox.ChildNodes[0]!.InnerXml.Replace("\n", "")
                            };

                            foreach (XmlNode options in checkbox.ChildNodes[1]!.ChildNodes)
                            {
                                var optionItem = new AppOptionModel
                                {
                                    IsChecked = bool.Parse(options.ChildNodes[0]!.InnerXml),
                                    Value = options.ChildNodes[1]!.InnerXml
                                };

                                newItem.Options.Add(optionItem);
                            }
                            newItem.TextItems = new ObservableCollection<AppFormDataItemTextModel?>();

                            if (checkbox.ChildNodes[2] is not null && checkbox.ChildNodes[2].Name == "texto")
                            {
                                var textItem = new AppFormDataItemTextModel()
                                {
                                    Topic = checkbox.ChildNodes[2].ChildNodes[0].InnerXml,
                                    Type = AppFormDataType.Texto,
                                    MeasurementUnit = checkbox.ChildNodes[2].ChildNodes[1].InnerXml,
                                    TextData = checkbox.ChildNodes[2].ChildNodes[2].InnerXml
                                };

                                newItem.TextItems.Add(textItem);
                            }

                            checkboxItem.Children.Add(newItem);
                        }
                        modelTable.Add(checkboxItem);
                    }

                    if ((AppFormDataType)type == AppFormDataType.Observação)
                    {
                        var value = (itemTable as XmlNode)!.ChildNodes[1]!.InnerXml;

                        var observation = new AppFormDataItemObservationModel()
                        {
                            Type = (AppFormDataType)type,
                            Observation = value,
                        };
                        modelTable.Add(observation);
                    }

                    if ((AppFormDataType)type == AppFormDataType.Image)
                    {
                        var value = (itemTable as XmlNode);

                        var image = new AppFormDataItemImageModel()
                        {
                            Type = (AppFormDataType)type,
                            ImagesItems = new List<ImagesItem>(),
                            Topic = "Imagens",
                        };

                        if ((itemTable as XmlNode)!.ChildNodes[1] is not null)
                        {
                            foreach (XmlNode item in (itemTable as XmlNode)!.ChildNodes[1])
                            {
                                image.ImagesItems.Add(new()
                                {
                                    imagePath = item?.ChildNodes[0]?.InnerXml,
                                    imageObservation = item?.ChildNodes[1]?.InnerXml,
                                });
                            }

                        }
                        modelTable.Add(image);
                    }
                }

                foreach (XmlNode itemLaw in root.ChildNodes[4]!.ChildNodes)
                {
                    var law = new AppLawModel
                    {
                        LawId = itemLaw.ChildNodes[0]?.InnerXml
                    };

                    var lawContent = new StringBuilder();

                    foreach (XmlNode item in itemLaw.ChildNodes[1].ChildNodes)
                    {
                        lawContent.AppendLine(item.InnerXml);
                    }

                    law.LawTextContent = lawContent.ToString();

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
        });

        task.Start();

        return await task;
    }
    public async Task SaveProjectItemContent(AppItemModel dataToWrite, string filePathToWrite)
    {
        await Task.Run(() =>
        {
            var doc = new XmlDocument();

            var root = doc.CreateElement("item");

            doc.AppendChild(root);

            var itemName = doc.CreateElement("name");
            itemName.InnerXml = dataToWrite.ItemName;

            var id = doc.CreateElement("id");
            id.InnerXml = dataToWrite.Id;

            var templateName = doc.CreateElement("templatename");
            templateName.InnerXml = dataToWrite.TemplateName;

            root.AppendChild(id);
            root.AppendChild(itemName);
            root.AppendChild(templateName);

            var tabelaName = doc.CreateElement("tabela");

            foreach (var projectItem in dataToWrite.FormData)
            {
                var attribute = doc.CreateAttribute("item");
                attribute.Value = $"{dataToWrite.FormData.IndexOf(projectItem) + 1}";

                var itemTabelaName = doc.CreateElement("itemtabela");

                itemTabelaName.Attributes.Append(attribute);

                if (projectItem.Type.Equals(AppFormDataType.Checkbox))
                {
                    var itemType = doc.CreateElement("tipo");
                    itemType.InnerXml = projectItem.Type.ToString()
                    .Replace(projectItem.Type.ToString()[0],
                    char.ToLower(projectItem.Type.ToString()[0]));

                    itemTabelaName.AppendChild(itemType);

                    var checkboxes = doc.CreateElement("checkboxes");

                    foreach (var checkbox in (projectItem as AppFormDataItemCheckboxModel)!.Children)
                    {
                        var checkboxItemAttribute = doc.CreateAttribute("item");
                        attribute.Value = $"{(projectItem as AppFormDataItemCheckboxModel)!.Children.IndexOf(checkbox) + 1}";

                        var itemCheckbox = doc.CreateElement("checkboxitem");
                        itemCheckbox.Attributes.Append(attribute);

                        var checkboxItemTopic = doc.CreateElement("topico");
                        checkboxItemTopic.InnerXml = checkbox.Topic;

                        itemCheckbox.AppendChild(checkboxItemTopic);

                        var options = doc.CreateElement("opcoes");

                        foreach (var option in checkbox.Options)
                        {
                            var itemOption = doc.CreateElement("opcao");

                            var optionItemAttribute = doc.CreateAttribute("item");
                            attribute.Value = $"{checkbox?.Options.IndexOf(option) + 1}";

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
                        {
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
                        }

                        checkboxes.AppendChild(itemCheckbox);
                    }

                    itemTabelaName.AppendChild(checkboxes);

                }

                if (projectItem.Type.Equals(AppFormDataType.Observação))
                {
                    var itemType = doc.CreateElement("tipo");
                    itemType.InnerXml = projectItem.Type.ToString()
                    .Replace(projectItem.Type.ToString()[0],
                    char.ToLower(projectItem.Type.ToString()[0]));


                    var itemObservationText = doc.CreateElement("value");
                    itemObservationText.InnerXml = (projectItem as AppFormDataItemObservationModel)!.Observation;

                    itemTabelaName.AppendChild(itemType);
                    itemTabelaName.AppendChild(itemObservationText);
                }

                if (projectItem.Type.Equals(AppFormDataType.Texto))
                {
                    var itemType = doc.CreateElement("tipo");
                    itemType.InnerXml = projectItem.Type.ToString()
                    .Replace(projectItem.Type.ToString()[0],
                    char.ToLower(projectItem.Type.ToString()[0]));

                    var itemTextoTopic = doc.CreateElement("topico");
                    itemTextoTopic.InnerXml = projectItem.Topic;

                    var itemTextoUnidade = doc.CreateElement("unidade");
                    itemTextoUnidade.InnerXml = (projectItem as AppFormDataItemTextModel)!.MeasurementUnit!;

                    var itemTextoValue = doc.CreateElement("value");
                    itemTextoValue.InnerXml = (projectItem as AppFormDataItemTextModel)!.TextData;

                    itemTabelaName.AppendChild(itemType);
                    itemTabelaName.AppendChild(itemTextoTopic);
                    itemTabelaName.AppendChild(itemTextoUnidade);
                    itemTabelaName.AppendChild(itemTextoValue);
                }

                if (projectItem.Type.Equals(AppFormDataType.Image))
                {
                    var itemType = doc.CreateElement("tipo");
                    itemType.InnerXml = projectItem.Type.ToString()
                    .Replace(projectItem.Type.ToString()[0],
                    char.ToLower(projectItem.Type.ToString()[0]));

                    var itemImageImages = doc.CreateElement("images");

                    if ((projectItem as AppFormDataItemImageModel)!.ImagesItems.Count > 0)
                    {
                        foreach (var item in (projectItem as AppFormDataItemImageModel)!.ImagesItems)
                        {
                            var itemContainer = doc.CreateElement("imageitem");
                            var itemImageImagesPath = doc.CreateElement("imagePath");
                            itemImageImagesPath.InnerXml = item.imagePath;

                            var itemImageObservation = doc.CreateElement("imageObservation");
                            itemImageObservation.InnerXml = item.imageObservation;

                            itemContainer.AppendChild(itemImageImagesPath);
                            itemContainer.AppendChild(itemImageObservation);

                            itemImageImages.AppendChild(itemContainer);
                        }
                    }

                    itemTabelaName.AppendChild(itemType);
                    itemTabelaName.AppendChild(itemImageImages);
                }

                tabelaName.AppendChild(itemTabelaName);
            }

            root.AppendChild(tabelaName);

            var leiName = doc.CreateElement("lei");

            foreach (var item in dataToWrite.LawList)
            {
                var itemLei = doc.CreateElement("leiitem");

                var itemLeiId = doc.CreateElement("id");
                itemLeiId.InnerXml = item.LawId;

                itemLei.AppendChild(itemLeiId);

                var LeiDescricao = doc.CreateElement("descricao");

                foreach (var itemLeiDes in item.LawTextContent.Split("\n"))
                {
                    var LeiItemDescricao = doc.CreateElement("itemdescricao");
                    LeiItemDescricao.InnerXml = itemLeiDes;

                    LeiDescricao.AppendChild(LeiItemDescricao);
                }

                itemLei.AppendChild(LeiDescricao);

                leiName.AppendChild(itemLei);
            }

            root.AppendChild(leiName);

            using (var writer = new StreamWriter(File.Create(filePathToWrite)))
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
    public async Task SaveSystemProjectItemContent(AppItemModel dataToWrite, string filePathToWrite)
    {
        await Task.Run(() =>
        {
            var doc = new XmlDocument();

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

                if (projectItem.Type.Equals(AppFormDataType.Checkbox))
                {
                    var itemType = doc.CreateElement("tipo");
                    itemType.InnerXml = projectItem.Type.ToString()
                    .Replace(projectItem.Type.ToString()[0],
                    char.ToLower(projectItem.Type.ToString()[0]));

                    itemTabelaName.AppendChild(itemType);

                    var checkboxes = doc.CreateElement("checkboxes");

                    foreach (var checkbox in (projectItem as AppFormDataItemCheckboxModel)!.Children)
                    {
                        var checkboxItemAttribute = doc.CreateAttribute("item");
                        attribute.Value = $"{(projectItem as AppFormDataItemCheckboxModel)!.Children.IndexOf(checkbox) + 1}";

                        var itemCheckbox = doc.CreateElement("checkboxitem");
                        itemCheckbox.Attributes.Append(attribute);

                        var checkboxItemTopic = doc.CreateElement("topico");
                        checkboxItemTopic.InnerXml = checkbox.Topic;

                        itemCheckbox.AppendChild(checkboxItemTopic);

                        var options = doc.CreateElement("opcoes");

                        foreach (var option in checkbox.Options)
                        {
                            var itemOption = doc.CreateElement("opcao");

                            var optionItemAttribute = doc.CreateAttribute("item");
                            attribute.Value = $"{checkbox?.Options.IndexOf(option) + 1}";

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
                        {
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
                        }

                        checkboxes.AppendChild(itemCheckbox);
                    }

                    itemTabelaName.AppendChild(checkboxes);

                }

                if (projectItem.Type.Equals(AppFormDataType.Observação))
                {
                    var itemType = doc.CreateElement("tipo");
                    itemType.InnerXml = projectItem.Type.ToString()
                    .Replace(projectItem.Type.ToString()[0],
                    char.ToLower(projectItem.Type.ToString()[0]));


                    var itemObservationText = doc.CreateElement("value");
                    itemObservationText.InnerXml = (projectItem as AppFormDataItemObservationModel)!.Observation;

                    itemTabelaName.AppendChild(itemType);
                    itemTabelaName.AppendChild(itemObservationText);
                }

                if (projectItem.Type.Equals(AppFormDataType.Texto))
                {
                    var itemType = doc.CreateElement("tipo");
                    itemType.InnerXml = projectItem.Type.ToString()
                    .Replace(projectItem.Type.ToString()[0],
                    char.ToLower(projectItem.Type.ToString()[0]));

                    var itemTextoTopic = doc.CreateElement("topico");
                    itemTextoTopic.InnerXml = projectItem.Topic;

                    var itemTextoUnidade = doc.CreateElement("unidade");
                    itemTextoUnidade.InnerXml = (projectItem as AppFormDataItemTextModel)!.MeasurementUnit!;

                    var itemTextoValue = doc.CreateElement("value");
                    itemTextoValue.InnerXml = (projectItem as AppFormDataItemTextModel)!.TextData;

                    itemTabelaName.AppendChild(itemType);
                    itemTabelaName.AppendChild(itemTextoTopic);
                    itemTabelaName.AppendChild(itemTextoUnidade);
                    itemTabelaName.AppendChild(itemTextoValue);
                }

                if (projectItem.Type.Equals(AppFormDataType.Image))
                {
                    var itemType = doc.CreateElement("tipo");
                    itemType.InnerXml = projectItem.Type.ToString()
                    .Replace(projectItem.Type.ToString()[0],
                    char.ToLower(projectItem.Type.ToString()[0]));

                    var itemImageImages = doc.CreateElement("images");

                    if ((projectItem as AppFormDataItemImageModel)!.ImagesItems.Count > 0)
                    {
                        foreach (var item in (projectItem as AppFormDataItemImageModel)!.ImagesItems)
                        {
                            var itemContainer = doc.CreateElement("imageitem");
                            var itemImageImagesPath = doc.CreateElement("imagePath");
                            itemImageImagesPath.InnerXml = item.imagePath;

                            var itemImageObservation = doc.CreateElement("imageObservation");
                            itemImageObservation.InnerXml = item.imageObservation;

                            itemContainer.AppendChild(itemImageImagesPath);
                            itemContainer.AppendChild(itemImageObservation);

                            itemImageImages.AppendChild(itemContainer);
                        }
                    }

                    itemTabelaName.AppendChild(itemType);
                    itemTabelaName.AppendChild(itemImageImages);
                }

                tabelaName.AppendChild(itemTabelaName);
            }

            root.AppendChild(tabelaName);

            var leiName = doc.CreateElement("lei");

            foreach (var item in dataToWrite.LawList)
            {
                var itemLei = doc.CreateElement("leiitem");

                var itemLeiId = doc.CreateElement("id");
                itemLeiId.InnerXml = item.LawId;

                itemLei.AppendChild(itemLeiId);

                var LeiDescricao = doc.CreateElement("descricao");

                foreach (var itemLeiDes in item.LawTextContent.Split("\n"))
                {
                    var LeiItemDescricao = doc.CreateElement("itemdescricao");
                    LeiItemDescricao.InnerXml = itemLeiDes;

                    LeiDescricao.AppendChild(LeiItemDescricao);
                }

                itemLei.AppendChild(LeiDescricao);

                leiName.AppendChild(itemLei);
            }

            root.AppendChild(leiName);

            using (var writer = new StreamWriter(filePathToWrite))
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
