﻿using Common;

using Core.Entities.Solution;
using Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using Core.Entities.Solution.Project.AppItem.DataItems.Images;
using Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using Core.Entities.Solution.Project.AppItem.DataItems.Text;

using ProjectItemReader.XmlFile;

using QuestPDF.Helpers;

using QuestPDFReport.Models;

namespace QuestPDFReport;
public static class DataSource
{
    public static async Task<IReport> GetReport(string path, string extension)
    {
        List<ReportHeaderField> HeaderFields()
        {
            return new List<ReportHeaderField>
                {
                    new ReportHeaderField()
                    {
                        Label = "Scope",
                        Value = "public activities"
                    },
                    new ReportHeaderField()
                    {
                        Label = "Author",
                        Value = "Marcin Ziąbek"
                    },
                    new ReportHeaderField()
                    {
                        Label = "Date",
                        Value = DateTime.Now.ToString("g")
                    },
                    new ReportHeaderField()
                    {
                        Label = "Status",
                        Value = "Completed, found 2 issues"
                    }
                };
        }

        var report = new ReportModel()
        {
            Title = "Sample Report Document",
            HeaderFields = HeaderFields(),
        };

        var res = Path.Combine(Directory.GetParent(path).FullName, Constants.AppProjectItemsFolderName);

        var items = Directory.GetFiles(res, $"*{extension}");

        var repository = new ProjectItemContentRepositoryImpl();

        foreach (var item in items)
        {
            var data = await repository.GetProjectItemContent(item);

            var section = new ReportSection();

            foreach (var formData in data.FormData)
            {
                section.Title = data.ItemName;

                if (formData is AppFormDataItemTextModel text)
                {
                    section.Parts.Add(new ReportSectionText(label: text.Topic, text: $"{text.TextData} {(text.MeasurementUnit is not null ? text.MeasurementUnit : "")}"));
                }

                if (formData is AppFormDataItemCheckboxModel checkbox)
                {
                    if (!string.IsNullOrWhiteSpace(checkbox.Topic))
                        section.Parts.Add(new ReportSectionTitle(checkbox.Topic));

                    foreach (var options in checkbox.Children)
                    {
                        var checkboxModels = options.Options.Select(item => new CheckboxModel(isChecked: item.IsChecked, value: item.Value));

                        var reportCheckbox = new ReportSectionCheckbox() { Label = options.Topic, Checkboxes = checkboxModels.ToList() };

                        section.Parts.Add(reportCheckbox);
                    }
                }

                if (formData is AppFormDataItemImageModel imageContainer)
                {

                    var container = new ReportSectionPhotoContainer()
                    {
                        Label = "Imagens",
                        Photos = imageContainer
                         .ImagesItems
                         .Select(x =>
                         new ReportSectionPhoto()
                         {
                             Path = x.imagePath,
                             Observation = x.imageObservation
                         })
                         .ToList()
                    };
                    section.Parts.Add(container);

                }
                if (formData is AppFormDataItemObservationModel observation)
                {
                    section.Parts.Add(new ReportSectionObservation()
                    {
                        Label = "Observações",
                        Observation = observation.Observation
                    });
                }
            }

            report.Sections.Add(section);
        }

        return report;
    }

    public static async Task<IReport> GetReport(ProjectSolutionModel solutionModel, string extension)
    {
        List<ReportHeaderField> HeaderFields()
        {
            return new List<ReportHeaderField>
                {
                    new ReportHeaderField()
                    {
                        Label = "Scope",
                        Value = "public activities"
                    },
                    new ReportHeaderField()
                    {
                        Label = "Author",
                        Value = "Marcin Ziąbek"
                    },
                    new ReportHeaderField()
                    {
                        Label = "Date",
                        Value = DateTime.Now.ToString("g")
                    },
                    new ReportHeaderField()
                    {
                        Label = "Status",
                        Value = "Completed, found 2 issues"
                    }
                };
        }

        var report = new NestedReportModel()
        {
            Title = "Sample Report Document",
            HeaderFields = HeaderFields(),
        };

        /* var res = Path.Combine(Directory.GetParent(solutionModel.FilePath).FullName, Constants.AppProjectItemsFolderName);*/

        var repository = new ProjectItemContentRepositoryImpl();

        foreach (var item in solutionModel.ItemGroups)
        {
            var sectionGroup = new ReportSectionGroup();

            sectionGroup.Title = item.Name;

            foreach (var itemModel in item.Items)
            {
                var data = await repository.GetProjectItemContent(itemModel.ItemPath);

                foreach (var formData in data.FormData)
                {
                    var section = new ReportSection();

                    section.Title = data.ItemName;

                    if (formData is AppFormDataItemTextModel text)
                    {
                        section.Parts.Add(new ReportSectionText(label: text.Topic, text: $"{text.TextData} {(text.MeasurementUnit is not null ? text.MeasurementUnit : "")}"));
                    }

                    if (formData is AppFormDataItemCheckboxModel checkbox)
                    {
                        if (!string.IsNullOrWhiteSpace(checkbox.Topic))
                            section.Parts.Add(new ReportSectionTitle(checkbox.Topic));

                        foreach (var options in checkbox.Children)
                        {
                            var checkboxModels = options.Options.Select(item => new CheckboxModel(isChecked: item.IsChecked, value: item.Value));

                            var reportCheckbox = new ReportSectionCheckbox() { Label = options.Topic, Checkboxes = checkboxModels.ToList() };

                            section.Parts.Add(reportCheckbox);
                        }
                    }

                    if (formData is AppFormDataItemImageModel imageContainer)
                    {
                        var container = new ReportSectionPhotoContainer()
                        {
                            Label = "Imagens",
                            Photos = imageContainer
                             .ImagesItems
                             .Select(x =>
                             new ReportSectionPhoto()
                             {
                                 Path = x.imagePath,
                                 Observation = x.imageObservation
                             })
                             .ToList()
                        };
                        section.Parts.Add(container);

                    }
                    if (formData is AppFormDataItemObservationModel observation)
                    {
                        section.Parts.Add(new ReportSectionObservation()
                        {
                            Label = "Observações",
                            Observation = observation.Observation
                        });
                    }
                    sectionGroup.Parts.Add(section);
                }

            }

            report.Sections.Add(sectionGroup);
        }

        return report;
    }

    public static IReport GetReport()
    {
        return new ReportModel
        {
            Title = "Sample Report Document",
            HeaderFields = HeaderFields(),

            LogoData = Helpers.GetImage("Logo.png"),
            Sections = Enumerable.Range(0, 40).Select(x => GenerateSection()).ToList(),
            /* Photos = Enumerable.Range(0, 25).Select(x => GetReportPhotos()).ToList()*/
        };

        List<ReportHeaderField> HeaderFields()
        {
            return new List<ReportHeaderField>
                {
                    new ReportHeaderField()
                    {
                        Label = "Scope",
                        Value = "public activities"
                    },
                    new ReportHeaderField()
                    {
                        Label = "Author",
                        Value = "Marcin Ziąbek"
                    },
                    new ReportHeaderField()
                    {
                        Label = "Date",
                        Value = DateTime.Now.ToString("g")
                    },
                    new ReportHeaderField()
                    {
                        Label = "Status",
                        Value = "Completed, found 2 issues"
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
                Parts = Enumerable.Range(0, sectionLength).Select(x => GetRandomElement()).ToList()
            };
        }

        ReportSectionElement GetRandomElement()
        {
            var random = Helpers.Random.NextDouble();

            if (random < 0.8f)
                return GetCheckboxElement();

            if (random < 0.85f)
                return GetTitleElement();

            if (random < 0.9f)
                return GetTextElement();

            if (random < 0.95f)
                return GetMapElement();

            return GetPhotosElement();
        }

        ReportSectionText GetTextElement()
        {
            return new ReportSectionText(label: Placeholders.Label(), text: Placeholders.Paragraph());
        }

        ReportSectionCheckbox GetCheckboxElement()
        {
            return new ReportSectionCheckbox
            {
                Label = Placeholders.Label(),
                Checkboxes = new List<CheckboxModel>
                {
                    new CheckboxModel(isChecked:false,value:Placeholders.Label()),
                    new CheckboxModel(isChecked:true,value:Placeholders.Label())
                }
            };
        }

        ReportSectionTitle GetTitleElement()
        {
            return new ReportSectionTitle(Placeholders.Label());
        }

        ReportSectionMap GetMapElement()
        {
            return new ReportSectionMap
            {
                Label = "Location",
                Location = Helpers.RandomLocation()
            };
        }

        ReportSectionPhotoContainer GetPhotosElement()
        {
            return new ReportSectionPhotoContainer
            {
                Label = "Photos",
                Photos = new()
            };
        }

        ReportPhoto GetReportPhotos()
        {
            return new ReportPhoto()
            {
                Comments = Placeholders.Sentence(),
                Date = DateTime.Now - TimeSpan.FromDays(Helpers.Random.NextDouble() * 100),
                Location = Helpers.RandomLocation()
            };
        }
    }
}
