using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

using AppUsecases.App.Contracts.Repositories;
using AppUsecases.Project.Contracts.Entity;
using AppUsecases.Project.Entities.Project;
using AppUsecases.Project.Entities.Project.AppFormDataItems.Checkbox;
using AppUsecases.Project.Entities.Project.AppFormDataItems.Observations;
using AppUsecases.Project.Entities.Project.AppFormDataItems.Text;
using AppUsecases.Project.Enums;

namespace ProjectItemReader.XmlFile;
public class ReadTemplateContentRepository : IReadContract<AppItemModel>
{
       public async Task<AppItemModel> ReadAsync(string path)
    {
        var task = new Task<AppItemModel>(() =>
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                using (StreamReader reader = new StreamReader(path))

                    doc.Load(reader);

                AppItemModel model = new();

                var root = doc.GetElementsByTagName("item")[0];

                model.ItemName = root!.ChildNodes[0]!.InnerXml.Replace("\n", "");

                var modelTable = new List<IAppFormDataItemContract>();

                var lawModel = new List<AppLawModel>();

                foreach (var itemTable in root.ChildNodes[1]?.ChildNodes)
                {
                    var res = (itemTable as XmlNode)?.ChildNodes[0]?.InnerXml;

                    Enum.TryParse(typeof(AppFormDataTypeEnum),
                        Regex.Replace(res, "^[a-z]", m => m.Value.ToUpper()),
                        out var type);

                    if ((AppFormDataTypeEnum)type! == AppFormDataTypeEnum.Text)
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
                            Type = (AppFormDataTypeEnum)type,
                            MeasurementUnit = unidadeText,
                            TextData = value,
                            Topic = topicoText
                        };
                        modelTable.Add(textItem);
                    }

                    if ((AppFormDataTypeEnum)type == AppFormDataTypeEnum.Checkbox)
                    {
                        var checkboxItem = new AppFormDataItemCheckboxModel()
                        {
                            Type = (AppFormDataTypeEnum)type,
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

                            var newItem = new AppFormDataItemCheckboxChildModel();
                            newItem.Options = new List<AppOptionModel>();

                            newItem.Topic = checkbox.ChildNodes[0]!.InnerXml.Replace("\n", "");

                            foreach (XmlNode options in checkbox.ChildNodes[1]!.ChildNodes)
                            {
                                var optionItem = new AppOptionModel();

                                optionItem.IsChecked = bool.Parse(options.ChildNodes[0]!.InnerXml);
                                optionItem.Value = options.ChildNodes[1]!.InnerXml;

                                newItem.Options.Add(optionItem);
                            }
                            newItem.TextItems = new List<AppFormDataItemTextModel?>();

                            if (checkbox.ChildNodes[2] is not null && checkbox.ChildNodes[2].Name == "texto")
                            {
                                var textItem = new AppFormDataItemTextModel()
                                {
                                    Topic = checkbox.ChildNodes[2].ChildNodes[0].InnerXml,
                                    Type = AppFormDataTypeEnum.Text,
                                    MeasurementUnit = checkbox.ChildNodes[2].ChildNodes[1].InnerXml,
                                    TextData = checkbox.ChildNodes[2].ChildNodes[2].InnerXml
                                };

                                newItem.TextItems.Add(textItem);
                            }

                            checkboxItem.Children.Add(newItem);
                        }
                        modelTable.Add(checkboxItem);
                    }

                    if ((AppFormDataTypeEnum)type == AppFormDataTypeEnum.Observation)
                    {
                        var value = (itemTable as XmlNode)!.ChildNodes[1]!.InnerXml;

                        var observation = new AppFormDataItemObservationModel()
                        {
                            Type = (AppFormDataTypeEnum)type,
                            Observation = value,
                        };
                        modelTable.Add(observation);
                    }

                }

                foreach (XmlNode itemLaw in root.ChildNodes[2]!.ChildNodes)
                {
                    var law = new AppLawModel();

                    law.LawId = itemLaw.ChildNodes[0]?.InnerXml;

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
}
