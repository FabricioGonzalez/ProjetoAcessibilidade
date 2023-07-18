using Common;
using Common.Linq;
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
            Title = "Sample Report Document", HeaderFields = HeaderFields()
        };

        var res = Path.Combine(Directory.GetParent(path).FullName
            , Constants.AppProjectItemsFolderName);

        var items = Directory.GetFiles(res, $"*{extension}");

        var repository = new ProjectItemContentRepositoryImpl();

        foreach (var item in items)
        {
            _ = (await repository.GetProjectItemContent(item)).IfSucc(data =>
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
                            section.Parts.Add(new ReportSectionTitle(checkbox.Topic, checkbox.Id));
                        }

                        foreach (var options in checkbox.Children)
                        {
                            var checkboxModels = options.Options.Select(optionChild =>
                                new CheckboxModel(optionChild.IsChecked, optionChild.Value, options.IsValid));

                            var reportCheckbox = new ReportSectionCheckbox(options.Topic, options.Id)
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
                                    new ReportSectionPhoto(x.ImageObservation, x.ImagePath, x.Id))
                                .ToList()
                        };
                        section.Parts.Add(container);
                    }

                    if (formData is AppFormDataItemObservationModel observation)
                    {
                        section.Parts.Add(
                            new ReportSectionObservation(
                                "Observações",
                                observation.Id,
                                observation.Observation));
                    }
                }

                report.Sections.Add(section);
            });
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
                }
                , new()
                {
                    Label = "UF", Value = "SP"
                }
                , new()
                {
                    Label = "Data", Value = solutionModel.SolutionReportInfo.Data.ToString("dd.MM.yyyy")
                }
                , new()
                {
                    Label = "Empresa", Value = solutionModel.SolutionReportInfo.NomeEmpresa
                }
                , new()
                {
                    Label = "Responsável pelo levantamento", Value = solutionModel.SolutionReportInfo.Responsavel
                }
                , new()
                {
                    Label = "E-mail", Value = solutionModel.SolutionReportInfo.Email
                }
                , new()
                {
                    Label = "Telefone", Value = solutionModel.SolutionReportInfo.Telefone
                }
                , new()
                {
                    Label = "Gerenciadora", Value = "solutionModel.StandartInfo.Gerenciadora;"
                }
            };
        }

        var report = new NestedReportModel
        {
            Title = "RELATÓRIO DE ACESSIBILIDADE", HeaderFields = HeaderFields(), StandardLaw =
                "Legislação Vigente: NBR 9.050/15, NBR 16.537/16, Decreto Nº 5296 de 02.12.2004 e Lei Federal 13.146/16"
        };

        /* var res = Path.Combine(Directory.GetParent(solutionModel.FilePath).FullName, Constants.AppProjectItemsFolderName);*/

        var repository = new ProjectItemContentRepositoryImpl();

        await solutionModel.LocationItems.IterateOnAsync(async locationItem =>
        {
            var Location = new ReportLocationGroup()
            {
                Title = locationItem.Name, Groups = new()
            };

            await locationItem.Items.IterateOnAsync(async groupItem =>
            {
                var sectionGroup = new ReportSectionGroup();

                sectionGroup.Title = groupItem.Name;
                sectionGroup.Id = Guid.NewGuid().ToString();

                foreach (var itemModel in groupItem.Items)
                {
                    _ = (await repository.GetProjectItemContent(itemModel.ItemPath))
                        .IfSucc(data =>
                        {
                            var section = new ReportSection();
                            section.Title = itemModel.Name;
                            section.Id = itemModel.Id;

                            _ = data.FormData.IterateOn(formData =>
                            {
                                if (formData is AppFormDataItemTextModel text)
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

                                if (formData is AppFormDataItemCheckboxModel checkbox)
                                {
                                    if (!string.IsNullOrWhiteSpace(checkbox.Topic))
                                    {
                                        section.Parts.Add(
                                            new ReportSectionTitle(
                                                checkbox.Topic,
                                                checkbox.Id
                                            ));
                                    }

                                    foreach (var options in checkbox.Children)
                                    {
                                        var checkboxModels = options
                                            .Options
                                            .Select(optionModel =>
                                                new CheckboxModel(optionModel.IsChecked, optionModel.Value
                                                    , options.IsValid));

                                        var reportCheckbox = new ReportSectionCheckbox(
                                            options.Topic,
                                            options.Id)
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
                                    ImagePath = image.ImagePath, Observation = image.ImageObservation
                                });
                            });

                            _ = data.Observations.IterateOn(observation =>
                            {
                                section.Observation = section.Observation.Append(new ObservationSectionElement
                                {
                                    Observation = observation.ObservationText
                                });
                            });

                            _ = data.LawList.IterateOn(law =>
                            {
                                section.Laws = section.Laws.Append(new LawSectionElement
                                {
                                    LawContent = law.LawTextContent, LawId = law.LawId
                                });
                            });

                            sectionGroup.Parts.Add(section);
                        });
                }

                Location.Groups.Add(sectionGroup);
            });
            report.Locations.Add(Location);
        });

        return report;
    }

    public static IReport GetReport()
    {
        return new ReportModel
        {
            Title = "Sample Report Document", HeaderFields = HeaderFields(), LogoData = Helpers.GetImage("Logo.png")
            , Sections = Enumerable.Range(0, 40).Select(x => GenerateSection()).ToList()
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
                ? Helpers.Random.Next(20, 40)
                : Helpers.Random.Next(5, 10);

            return new ReportSection
            {
                Title = Placeholders.Label(), Parts = Enumerable.Range(0, sectionLength)
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
            return new ReportSectionCheckbox(Placeholders.Label(), Guid.NewGuid().ToString())
            {
                Checkboxes = new List<CheckboxModel>
                {
                    new(false, Placeholders.Label()), new(true, Placeholders.Label())
                }
            };
        }

        ReportSectionTitle GetTitleElement()
        {
            return new ReportSectionTitle(Placeholders.Label(), Guid.NewGuid().ToString());
        }

        ReportSectionMap GetMapElement()
        {
            return new ReportSectionMap("Location", Guid.NewGuid().ToString())
            {
                Location = Helpers.RandomLocation()
            };
        }

        ReportSectionPhotoContainer GetPhotosElement()
        {
            return new ReportSectionPhotoContainer
            {
                Label = "Photos", Photos = new List<ReportSectionPhoto>()
            };
        }

        ReportPhoto GetReportPhotos()
        {
            return new ReportPhoto
            {
                Comments = Placeholders.Sentence()
                , Date = DateTime.Now - TimeSpan.FromDays(Helpers.Random.NextDouble() * 100)
                , Location = Helpers.RandomLocation()
            };
        }
    }
}