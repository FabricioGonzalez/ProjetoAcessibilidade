using Common;
using Common.Helpers;
using Common.Linq;

using LanguageExt;

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
                },
                new()
                {
                    Label = "Author", Value = "Marcin Ziąbek"
                },
                new()
                {
                    Label = "Date", Value = DateTime.Now.ToString("g")
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
            HeaderFields = new FieldsContainer()
        };

        var res = Path.Combine(Directory.GetParent(path).FullName
            , Constants.AppProjectItemsFolderName);

        var items = Directory.GetFiles(res, $"*{extension}");

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
    )
    {
        var report = new NestedReportModel
        {
            Title = "RELATÓRIO DE ACESSIBILIDADE",
            HeaderFields = new FieldsContainer
            {
                Local = new ReportHeaderField
                {
                    Label = "Local",
                    Value =
                        $"{solutionModel.Report.CompanyInfo.Endereco.Logradouro},{solutionModel.Report.CompanyInfo.Endereco.Numero},{solutionModel.Report.CompanyInfo.Endereco.Bairro},{solutionModel.Report.CompanyInfo.Endereco.Cidade}"
                },
                Revisao = new ReportHeaderField
                {
                    Label = "Revisão",
                    Value = solutionModel.Report.Revisao.ToString()
                },
                Uf = new ReportHeaderField
                {
                    Label = "UF",
                    Value = $"{solutionModel.Report.CompanyInfo.Endereco.UF}"
                },
                Data = new ReportHeaderField
                {
                    Label = "Data da Vistoria",
                    Value = solutionModel.Report.Manager.ReportDate.ToString("dd.MM.yyyy")
                },
                Empresa = new ReportHeaderField
                {
                    Label = "Empresa",
                    Value = solutionModel.Report.CompanyInfo.NomeEmpresa
                },
                Responsavel = new ReportHeaderField
                {
                    Label = "Responsável pelo levantamento",
                    Value = solutionModel.Report.Manager.Responsavel
                },
                Email = new ReportHeaderField
                {
                    Label = "E-mail",
                    Value = solutionModel.Report.CompanyInfo.Email
                },
                Telefone = new ReportHeaderField
                {
                    Label = "Telefone",
                    Value = solutionModel.Report.Manager.Telefone
                },
                Gerenciadora = new ReportHeaderField
                {
                    Label = "Gerenciadora",
                    Value = solutionModel.Report.Manager.NomeEmpresa
                }
            },
            StandardLaw =
                standardLaw,
            Partners = solutionModel.Report.Partners.Prepend(new PartnerItem
            {
                NomeEmpresa = solutionModel.Report.Manager.NomeEmpresa,
                PartnerLogo = solutionModel.Report.Manager.LogoPath,
                WebSite = solutionModel.Report.Manager.WebSite
            }),
            CompanyInfo = solutionModel.Report.CompanyInfo,
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
                var sectionGroup = new ReportSectionGroup
                {
                    Title = groupItem.Name,
                    Id = Guid.NewGuid().ToString()
                };

                foreach (var itemModel in groupItem.ItemsGroup)
                {
                    var path = Path.Combine(string.Join(Path.DirectorySeparatorChar, solutionModel.SolutionPath.
                   Split(Path.DirectorySeparatorChar)[..^1]),
                  string.Join(Path.DirectorySeparatorChar, itemModel.ItemPath.Split(Path.DirectorySeparatorChar)[^4..]))
                    .ExistsOrDefault(itemModel.ItemPath);

                    (await repository.GetContentItem(path))
                    .IfSucc(data =>
                    {
                        var section = new ReportSection();
                        section.Title = itemModel.Name;
                        section.Id = itemModel.Id;

                        data.FormData?.IterateOn(formData =>
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
                                            checkbox.Topic,
                                            checkbox.Id
                                        ));
                                }

                                foreach (var checkboxItem in checkbox.Children)
                                {
                                    var checkboxModels = checkboxItem
                                        .Options
                                        .Select(optionModel =>
                                            new CheckboxModel(optionModel.IsChecked
                                                , optionModel.Value
                                                , optionModel.IsInvalid));

                                    var reportCheckbox = new ReportSectionCheckbox(
                                        checkboxItem.Topic,
                                        checkboxItem.Id)
                                    {
                                        Checkboxes = checkboxModels.ToList(),
                                        TextItems = checkboxItem
                                            .TextItems
                                            .Select(it =>
                                                new TextModel(it.TextData, it.Topic, it.MeasurementUnit))
                                            .ToList()
                                    };

                                    section.Parts.Add(reportCheckbox);
                                }
                            }
                        });

                        data.Images.IterateOn(image =>
                        {
                            section.Images = section.Images.Append(new ImageSectionElement
                            {
                                ImagePath = image.ImagePath,
                                Observation = image.ImageObservation
                            });
                        });

                        data.Observations.IterateOn(observation =>
                        {
                            section.Observation = section.Observation.Append(new ObservationSectionElement
                            {
                                Observation = observation.Observation
                            });
                        });

                        data.LawList.IterateOn(law =>
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

        if (!File.Exists(Path.Combine(Directory.GetParent(solutionModel.SolutionPath).FullName
                , "conclusion.prjc")))
        {
            File.Create(Path.Combine(Directory.GetParent(solutionModel.SolutionPath).FullName
                , "conclusion.prjc")).Close();
        }

        report.Conclusion =
            File.ReadAllText(Path.Combine(Directory.GetParent(solutionModel.SolutionPath).FullName
                , "conclusion.prjc"));

        return report;
    }

    public static IReport GetReport()
    {
        return new ReportModel
        {
            Title = "Sample Report Document",
            HeaderFields = new FieldsContainer(),
            LogoData = Helpers.GetImage("Logo.png"),
            Sections = Enumerable.Range(0, 40).Select(x => GenerateSection()).ToList()
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
                    Label = "Date", Value = DateTime.Now.ToString("g")
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
                ? Helpers.Random.Next(20, 40)
                : Helpers.Random.Next(5, 10);

            return new ReportSection
            {
                Title = Placeholders.Label(),
                Parts = Enumerable.Range(0, sectionLength)
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
                    new(false, Placeholders.Label()),
                    new(true, Placeholders.Label())
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
                Label = "Photos",
                Photos = new List<ReportSectionPhoto>()
            };
        }

        ReportPhoto GetReportPhotos()
        {
            return new ReportPhoto
            {
                Comments = Placeholders.Sentence(),
                Date = DateTime.Now - TimeSpan.FromDays(Helpers.Random.NextDouble() * 100),
                Location = Helpers.RandomLocation()
            };
        }
    }
}