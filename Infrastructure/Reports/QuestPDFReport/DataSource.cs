using Common;
using Common.Linq;

using Core.Entities.Solution.Project.AppItem;
using Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;

using ProjectItemReader.XmlFile;

using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Images;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Text;

using QuestPDF.Helpers;

using QuestPDFReport.Models;

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
                },
                new()
                {
                    Label = "Author", Value = "Marcin Ziąbek"
                },
                new()
                {
                    Label = "Date", Value = DateTime.Now.ToString(format: "g")
                },
                new()
                {
                    Label = "Status", Value = "Completed, found 2 issues"
                }
            };
        }

        var report = new ReportModel
        {
            Title = "Sample Report Document",
            HeaderFields = HeaderFields()
        };

        var res = Path.Combine(path1: Directory.GetParent(path: path).FullName
            , path2: Constants.AppProjectItemsFolderName);

        var items = Directory.GetFiles(path: res, searchPattern: $"*{extension}");

        var repository = new ProjectItemContentRepositoryImpl();

        foreach (var item in items)
        {
            AppItemModel? data = await repository.GetProjectItemContent(filePathToRead: item);

            var section = new ReportSection();

            foreach (var formData in data.FormData)
            {
                section.Title = data.ItemName;

                if (formData is AppFormDataItemTextModel text)
                {
                    section.Parts.Add(item: new ReportSectionText(label: text.Topic,
                        text: $"{text.TextData}",
                        measurementUnit: text.MeasurementUnit is not null ? text.MeasurementUnit : "",
                        id: text.Id));
                }

                if (formData is AppFormDataItemCheckboxModel checkbox)
                {
                    if (!string.IsNullOrWhiteSpace(value: checkbox.Topic))
                    {
                        section.Parts.Add(item: new ReportSectionTitle(Topic: checkbox.Topic, id: checkbox.Id));
                    }

                    foreach (var options in checkbox.Children)
                    {
                        var checkboxModels = options.Options.Select(selector: item =>
                            new CheckboxModel(isChecked: item.IsChecked, value: item.Value));

                        var reportCheckbox = new ReportSectionCheckbox(label: options.Topic, id: options.Id)
                        {
                            Checkboxes = checkboxModels.ToList()
                        };

                        section.Parts.Add(item: reportCheckbox);
                    }
                }

                if (formData is AppFormDataItemImageModel imageContainer)
                {
                    var container = new ReportSectionPhotoContainer
                    {
                        Label = "Imagens",
                        Photos = imageContainer
                            .ImagesItems
                            .Select(selector: x =>
                                new ReportSectionPhoto(observation: x.ImageObservation, path: x.ImagePath, id: x.Id))
                            .ToList()
                    };
                    section.Parts.Add(item: container);
                }

                if (formData is AppFormDataItemObservationModel observation)
                {
                    section.Parts.Add(
                        item: new ReportSectionObservation(
                            label: "Observações",
                            id: observation.Id,
                            observation: observation.Observation));
                }
            }

            report.Sections.Add(item: section);
        }

        return report;
    }

    public static async Task<IReport> GetReport(
        ProjectSolutionModel solutionModel
        , string extension
    )
    {
        List<ReportHeaderField> HeaderFields()
        {
            return new List<ReportHeaderField>
            {
                new()
                {
                    Label = "Local", Value = solutionModel.SolutionReportInfo.Endereco
                },
                new()
                {
                    Label = "UF", Value = "SP"
                },
                new()
                {
                    Label = "Data", Value = solutionModel.SolutionReportInfo.Data.ToString(format: "dd.MM.yyyy")
                },
                new()
                {
                    Label = "Empresa", Value = solutionModel.SolutionReportInfo.NomeEmpresa
                },
                new()
                {
                    Label = "Responsável pelo levantamento", Value = solutionModel.SolutionReportInfo.Responsavel
                },
                new()
                {
                    Label = "E-mail", Value = solutionModel.SolutionReportInfo.Email
                },
                new()
                {
                    Label = "Telefone", Value = solutionModel.SolutionReportInfo.Telefone
                },
                new()
                {
                    Label = "Gerenciadora", Value = "solutionModel.StandartInfo.Gerenciadora;"
                }
            };
        }

        var report = new NestedReportModel
        {
            Title = "RELATÓRIO DE ACESSIBILIDADE",
            HeaderFields = HeaderFields(),
            StandardLaw =
                "Legislação Vigente: NBR 9.050/15, NBR 16.537/16, Decreto Nº 5296 de 02.12.2004 e Lei Federal 13.146/16"
        };

        /* var res = Path.Combine(Directory.GetParent(solutionModel.FilePath).FullName, Constants.AppProjectItemsFolderName);*/

        var repository = new ProjectItemContentRepositoryImpl();

        foreach (var item in solutionModel.ItemGroups)
        {
            var sectionGroup = new ReportSectionGroup();

            sectionGroup.Title = item.Name;
            sectionGroup.Id = Guid.NewGuid().ToString();

            foreach (var itemModel in item.Items)
            {
                var data = await repository.GetProjectItemContent(filePathToRead: itemModel.ItemPath);

                var section = new ReportSection();
                section.Title = itemModel.Name;
                section.Id = itemModel.Id;

                data.FormData.IterateOn((formData) =>
                {
                    if (formData is AppFormDataItemTextModel text)
                    {
                        section
                            .Parts
                            .Add(item: new ReportSectionText(
                                label: text.Topic,
                                text:
                                $"{text.TextData}",
                               measurementUnit: text.MeasurementUnit is not null ? text.MeasurementUnit : "",
                                id: text.Id));
                    }

                    if (formData is AppFormDataItemCheckboxModel checkbox)
                    {
                        if (!string.IsNullOrWhiteSpace(value: checkbox.Topic))
                        {
                            section.Parts.Add(
                                item: new ReportSectionTitle(
                                    Topic: checkbox.Topic,
                                    id: checkbox.Id
                                ));
                        }

                        foreach (var options in checkbox.Children)
                        {
                            var checkboxModels = options
                                .Options
                                .Select(selector: item =>
                                    new CheckboxModel(isChecked: item.IsChecked, value: item.Value));

                            var reportCheckbox = new ReportSectionCheckbox(
                                label: options.Topic,
                                id: options.Id)
                            {
                                Checkboxes = checkboxModels.ToList()
                            };

                            section.Parts.Add(item: reportCheckbox);
                        }
                    }
                });

                data.Images.IterateOn((image) =>
                {
                    section.Images = section.Images.Append(new()
                    {
                        ImagePath = image.ImagePath,
                        Observation = image.ImageObservation
                    });
                });

                data.Observations.IterateOn((observation) =>
                {
                    section.Observation = section.Observation.Append(new()
                    {
                        Observation = observation.ObservationText
                    });
                });

                data.LawList.IterateOn((law) =>
               {
                   section.Laws = section.Laws.Append(new()
                   {
                       LawContent = law.LawTextContent,
                       LawId = law.LawId
                   });
               });

                foreach (var formData in data.FormData)
                {


                    /*if (formData is AppFormDataItemImageModel imageContainer)
                    {
                        var container = new ReportSectionPhotoContainer
                        {
                            Label = "Imagens",
                            Photos = imageContainer
                                .ImagesItems
                                .Select(selector: x =>
                                    new ReportSectionPhoto(
                                        observation: x.ImageObservation,
                                        path: x.ImagePath,
                                        id: x.Id)
                                )
                                .ToList()
                        };
                        section.Parts.Add(item: container);
                    }

                    if (formData is AppFormDataItemObservationModel observation)
                    {
                        section.Parts.Add(
                            item: new ReportSectionObservation(
                                label: "Observações",
                                id: observation.Id,
                                observation: observation.Observation));
                    }*/
                }

                sectionGroup.Parts.Add(item: section);
            }

            report.Sections.Add(item: sectionGroup);
        }

        return report;
    }

    public static IReport GetReport()
    {
        return new ReportModel
        {
            Title = "Sample Report Document",
            HeaderFields = HeaderFields(),
            LogoData = Helpers.GetImage(name: "Logo.png"),
            Sections = Enumerable.Range(start: 0, count: 40).Select(selector: x => GenerateSection()).ToList()
            /* Photos = Enumerable.Range(0, 25).Select(x => GetReportPhotos()).ToList()*/
        };

        List<ReportHeaderField> HeaderFields()
        {
            return new List<ReportHeaderField>
            {
                new()
                {
                    Label = "Scope", Value = "public activities"
                },
                new()
                {
                    Label = "Author", Value = "Marcin Ziąbek"
                },
                new()
                {
                    Label = "Date", Value = DateTime.Now.ToString(format: "g")
                },
                new()
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
                    .Select(selector: x => GetRandomElement())
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
                    new(isChecked: false, value: Placeholders.Label()),
                    new(isChecked: true, value: Placeholders.Label())
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
                Comments = Placeholders.Sentence(),
                Date = DateTime.Now - TimeSpan.FromDays(value: Helpers.Random.NextDouble() * 100),
                Location = Helpers.RandomLocation()
            };
        }
    }
}