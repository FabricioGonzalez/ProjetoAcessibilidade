using Common;
using Common.Linq;

using QuestPDF.Helpers;

using QuestPDFReport.Models;

using XmlDatasource.ProjectItems;
using XmlDatasource.ProjectItems.DTO.FormItem;
using XmlDatasource.Solution.DTO;

namespace QuestPDFReport;

public static class DataSource
{
    public static async Task<IReport> GetReport(
        string path
        , string extension
    )
    {
        List<ReportHeaderField> HeaderFields()
        {
            return new List<ReportHeaderField>
            {
                new()
                {
                    Label = "Scope", Value = "public activities"
                }
                , new()
                {
                    Label = "Author", Value = "Marcin Ziąbek"
                }
                , new()
                {
                    Label = "Date", Value = DateTime.Now.ToString("g")
                }
                , new()
                {
                    Label = "Status", Value = "Completed, found 2 issues"
                }
            };
        }

        var report = new ReportModel
        {
            Title = "Sample Report Document",
            HeaderFields = new FieldsContainer()
        };

        var res = Path.Combine(path1: Directory.GetParent(path).FullName
            , path2: Constants.AppProjectItemsFolderName);

        var items = Directory.GetFiles(path: res, searchPattern: $"*{extension}");

        /*var repository = new ProjectItemContentRepositoryImpl();*/

        foreach (var item in items)
        {
            /*(await repository.GetProjectItemContent(item)).IfSucc(data =>
            {
                var section = new ReportSection();

                foreach (var formData in data.FormData)
                {
                    section.Title = data.ItemName;

                    if (formData is AppFormDataItemTextModel text)
                    {
                        section.Parts.Add(new ReportSectionText(label: text.Topic,
                            text: $"{text.TextData}",
                            measurementUnit: text.MeasurementUnit ?? "",
                            id: text.Id));
                    }

                    if (formData is AppFormDataItemCheckboxModel checkbox)
                    {
                        if (!string.IsNullOrWhiteSpace(checkbox.Topic))
                        {
                            section.Parts.Add(new ReportSectionTitle(Topic: checkbox.Topic, id: checkbox.Id));
                        }

                        foreach (var options in checkbox.Children)
                        {
                            var checkboxModels = options.Options.Select(optionChild =>
                                new CheckboxModel(isChecked: optionChild.IsChecked, value: optionChild.Value
                                    , isValid: options.IsValid));

                            var reportCheckbox = new ReportSectionCheckbox(label: options.Topic, id: options.Id)
                            {
                                Checkboxes = checkboxModels.ToList()
                            };

                            section.Parts.Add(reportCheckbox);
                        }
                    }

                    if (formData is AppFormDataItemImageModel imageContainer)
                    {
                        var container = new ReportSectionPhotoContainer
                        {
                            Label = "Imagens", Photos = imageContainer
                                .ImagesItems
                                .Select(x =>
                                    new ReportSectionPhoto(observation: x.ImageObservation, path: x.ImagePath
                                        , id: x.Id))
                                .ToList()
                        };
                        section.Parts.Add(container);
                    }

                    if (formData is AppFormDataItemObservationModel observation)
                    {
                        section.Parts.Add(
                            new ReportSectionObservation(
                                label: "Observações",
                                id: observation.Id,
                                observation: observation.Observation));
                    }
                }

                report.Sections.Add(section);
            });*/
        }

        return report;
    }

    public static async Task<IReport> GetReport(
        SolutionItemRoot solutionModel,
        string standardLaw
    /*, string extension*/
    )
    {
        var report = new NestedReportModel
        {
            Title = "RELATÓRIO DE ACESSIBILIDADE",
            HeaderFields = new FieldsContainer
            {
                Local = new ReportHeaderField
                {
                    Label = "Local"
                    ,
                    Value =
                        $"{solutionModel.Report.CompanyInfo.Endereco.Logradouro},{solutionModel.Report.CompanyInfo.Endereco.Numero},{solutionModel.Report.CompanyInfo.Endereco.Bairro},{solutionModel.Report.CompanyInfo.Endereco.Cidade}"
                }
                ,
                Revisao = new ReportHeaderField
                {
                    Label = "Revisão"
                    ,
                    Value = solutionModel.Report.Revisao.ToString()
                }
                ,
                Uf = new ReportHeaderField
                {
                    Label = "UF",
                    Value = $"{solutionModel.Report.CompanyInfo.Endereco.UF}"
                }
                ,
                Data = new ReportHeaderField
                {
                    Label = "Data da Vistoria",
                    Value = solutionModel.Report.CompanyInfo.Data.ToString("dd.MM.yyyy")
                }
                ,
                Empresa = new ReportHeaderField
                {
                    Label = "Empresa",
                    Value = solutionModel.Report.CompanyInfo.NomeEmpresa
                }
                ,
                Responsavel = new ReportHeaderField
                {
                    Label = "Responsável pelo levantamento",
                    Value = solutionModel.Report.Manager.Responsavel
                }
                ,
                Email = new ReportHeaderField
                {
                    Label = "E-mail",
                    Value = solutionModel.Report.CompanyInfo.Email
                }
                ,
                Telefone = new ReportHeaderField
                {
                    Label = "Telefone",
                    Value = solutionModel.Report.Manager.Telefone
                }
                ,
                Gerenciadora = new ReportHeaderField
                {
                    Label = "Gerenciadora",
                    Value = solutionModel.Report.Manager.NomeEmpresa
                }
            }
            ,
            StandardLaw =
              standardLaw
            ,
            Partners = solutionModel.Report.Partners,
            CompanyInfo = solutionModel.Report.CompanyInfo
            ,
            ManagerInfo = solutionModel.Report.Manager
        };

        var repository = new ProjectItemDatasourceImpl();

        await solutionModel.ProjectItems.IterateOnAsync(async locationItem =>
        {
            var Location = new ReportLocationGroup
            {
                Title = locationItem.ItemName,
                Groups = new List<ReportSectionGroup>()
            };

            await locationItem.LocationGroups.IterateOnAsync(async groupItem =>
            {
                var sectionGroup = new ReportSectionGroup();

                sectionGroup.Title = groupItem.Name;
                sectionGroup.Id = Guid.NewGuid().ToString();

                foreach (var itemModel in groupItem.ItemsGroup)
                {
                    (await repository.GetContentItem(itemModel.ItemPath))
                        .IfSucc(data =>
                        {
                            var section = new ReportSection();
                            section.Title = itemModel.Name;
                            section.Id = itemModel.Id;

                            data.FormData.IterateOn(formData =>
                            {
                                if (formData is ItemFormDataTextModel text)
                                {
                                    section
                                        .Parts
                                        .Add(new ReportSectionText(
                                            label: text.Topic,
                                            text:
                                            $"{text.TextData}",
                                            measurementUnit: text.MeasurementUnit ?? "",
                                            id: text.Id));
                                }

                                if (formData is ItemFormDataCheckboxModel checkbox)
                                {
                                    if (!string.IsNullOrWhiteSpace(checkbox.Topic))
                                    {
                                        section.Parts.Add(
                                            new ReportSectionTitle(
                                                Topic: checkbox.Topic,
                                                id: checkbox.Id
                                            ));
                                    }

                                    foreach (var options in checkbox.Children)
                                    {
                                        var checkboxModels = options
                                            .Options
                                            .Select(optionModel =>
                                                new CheckboxModel(isChecked: optionModel.IsChecked
                                                    , value: optionModel.Value
                                                    , isValid: optionModel.IsInvalid));

                                        var reportCheckbox = new ReportSectionCheckbox(
                                            label: options.Topic,
                                            id: options.Id)
                                        {
                                            Checkboxes = checkboxModels.ToList()
                                        };

                                        section.Parts.Add(reportCheckbox);
                                    }
                                }
                            });

                            _ = data.Images.IterateOn(image =>
                            {
                                section.Images = section.Images.Append(new ImageSectionElement
                                {
                                    ImagePath = image.ImagePath,
                                    Observation = image.ImageObservation
                                });
                            });

                            _ = data.Observations.IterateOn(observation =>
                            {
                                section.Observation = section.Observation.Append(new ObservationSectionElement
                                {
                                    Observation = observation.Observation
                                });
                            });

                            _ = data.LawList.IterateOn(law =>
                            {
                                section.Laws = section.Laws.Append(new LawSectionElement
                                {
                                    LawContent = law.LawTextContent,
                                    LawId = law.LawId
                                });
                            });

                            sectionGroup.Parts.Add(section);
                        });
                }

                Location.Groups.Add(sectionGroup);
            });
            report.Locations.Add(Location);
        });

        if (!File.Exists(Path.Combine(path1: Directory.GetParent(solutionModel.SolutionPath).FullName
                , path2: "conclusion.prjc")))
        {
            File.Create(Path.Combine(path1: Directory.GetParent(solutionModel.SolutionPath).FullName
                , path2: "conclusion.prjc")).Close();
        }

        report.Conclusion =
            File.ReadAllText(Path.Combine(path1: Directory.GetParent(solutionModel.SolutionPath).FullName
                , path2: "conclusion.prjc"));

        return report;
    }

    public static IReport GetReport()
    {
        return new ReportModel
        {
            Title = "Sample Report Document",
            HeaderFields = new FieldsContainer()
            ,
            LogoData = Helpers.GetImage("Logo.png")
            ,
            Sections = Enumerable.Range(start: 0, count: 40).Select(x => GenerateSection()).ToList()
            /* Photos = Enumerable.Range(0, 25).Select(x => GetReportPhotos()).ToList()*/
        };

        List<ReportHeaderField> HeaderFields()
        {
            return new List<ReportHeaderField>
            {
                new()
                {
                    Label = "Scope", Value = "public activities"
                }
                , new()
                {
                    Label = "Author", Value = "Marcin Ziąbek"
                }
                , new()
                {
                    Label = "Date", Value = DateTime.Now.ToString("g")
                }
                , new()
                {
                    Label = "Status", Value = "Completed, found 2 issues"
                }
            };
        }

        ReportSection GenerateSection()
        {
            var sectionLength = Helpers.Random.NextDouble() > 0.75
                ? Helpers.Random.Next(minValue: 20, maxValue: 40)
                : Helpers.Random.Next(minValue: 5, maxValue: 10);

            return new ReportSection
            {
                Title = Placeholders.Label(),
                Parts = Enumerable.Range(start: 0, count: sectionLength)
                    .Select(x => GetRandomElement())
                    .ToList()
            };
        }

        ReportSectionElement GetRandomElement()
        {
            var random = Helpers.Random.NextDouble();

            if (random < 0.8f)
            {
                return GetCheckboxElement();
            }

            if (random < 0.85f)
            {
                return GetTitleElement();
            }

            if (random < 0.9f)
            {
                return GetTextElement();
            }

            if (random < 0.95f)
            {
                return GetMapElement();
            }

            return GetPhotosElement();
        }

        ReportSectionText GetTextElement()
        {
            return new ReportSectionText(
                label: Placeholders.Label(),
                text: Placeholders.Paragraph(),
                measurementUnit: "",
                id: Guid.NewGuid().ToString());
        }

        ReportSectionCheckbox GetCheckboxElement()
        {
            return new ReportSectionCheckbox(label: Placeholders.Label(), id: Guid.NewGuid().ToString())
            {
                Checkboxes = new List<CheckboxModel>
                {
                    new(isChecked: false, value: Placeholders.Label())
                    , new(isChecked: true, value: Placeholders.Label())
                }
            };
        }

        ReportSectionTitle GetTitleElement()
        {
            return new ReportSectionTitle(Topic: Placeholders.Label(), id: Guid.NewGuid().ToString());
        }

        ReportSectionMap GetMapElement()
        {
            return new ReportSectionMap(label: "Location", id: Guid.NewGuid().ToString())
            {
                Location = Helpers.RandomLocation()
            };
        }

        ReportSectionPhotoContainer GetPhotosElement()
        {
            return new ReportSectionPhotoContainer
            {
                Label = "Photos",
                Photos = new List<ReportSectionPhoto>()
            };
        }

        ReportPhoto GetReportPhotos()
        {
            return new ReportPhoto
            {
                Comments = Placeholders.Sentence()
                ,
                Date = DateTime.Now - TimeSpan.FromDays(Helpers.Random.NextDouble() * 100)
                ,
                Location = Helpers.RandomLocation()
            };
        }
    }
}