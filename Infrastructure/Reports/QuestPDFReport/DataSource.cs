using Common;

using Core.Entities.Solution;
using Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using Core.Entities.Solution.Project.AppItem.DataItems.Images;
using Core.Entities.Solution.Project.AppItem.DataItems.Observations;
using Core.Entities.Solution.Project.AppItem.DataItems.Text;

using ProjectItemReader.XmlFile;
using ProjectItemReader.XmlFile.DTO.FormItem;

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
                    section.Parts.Add(new ReportSectionText(label: text.Topic,
                        text: $"{text.TextData} {(text.MeasurementUnit is not null ? text.MeasurementUnit : "")}", id: text.Id));
                }

                if (formData is AppFormDataItemCheckboxModel checkbox)
                {
                    if (!string.IsNullOrWhiteSpace(checkbox.Topic))
                        section.Parts.Add(new ReportSectionTitle(checkbox.Topic, id: checkbox.Id));

                    foreach (var options in checkbox.Children)
                    {
                        var checkboxModels = options.Options.Select(item => new CheckboxModel(isChecked: item.IsChecked, value: item.Value));

                        var reportCheckbox = new ReportSectionCheckbox(label: options.Topic, id: options.Id) { Checkboxes = checkboxModels.ToList() };

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

                var section = new ReportSection();
                section.Title = itemModel.Name;

                foreach (var formData in data.FormData)
                {

                    if (formData is AppFormDataItemTextModel text)
                    {
                        section
                            .Parts
                            .Add(new ReportSectionText(
                                label: text.Topic,
                            text: $"{text.TextData} {(text.MeasurementUnit is not null ? text.MeasurementUnit : "")}",
                            id: text.Id));
                    }

                    if (formData is AppFormDataItemCheckboxModel checkbox)
                    {
                        if (!string.IsNullOrWhiteSpace(checkbox.Topic))
                            section.Parts.Add(
                                item: new ReportSectionTitle(
                                    Topic: checkbox.Topic,
                                    id: checkbox.Id
                                    ));

                        foreach (var options in checkbox.Children)
                        {
                            var checkboxModels = options
                                .Options
                                .Select(item =>
                                new CheckboxModel(isChecked: item.IsChecked, value: item.Value));

                            var reportCheckbox = new ReportSectionCheckbox(
                                label: options.Topic,
                                id: options.Id)
                            {
                                Checkboxes = checkboxModels.ToList()
                            };

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
                             new ReportSectionPhoto(
                                 observation: x.ImageObservation,
                             path: x.ImagePath,
                             id: x.Id)
                            )
                             .ToList()
                        };
                        section.Parts.Add(container);

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

                sectionGroup.Parts.Add(section);

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
            return new ReportSectionText(label: Placeholders.Label(), text: Placeholders.Paragraph(), id: Guid.NewGuid().ToString());
        }

        ReportSectionCheckbox GetCheckboxElement()
        {
            return new ReportSectionCheckbox(label: Placeholders.Label(), id: Guid.NewGuid().ToString())
            {
                Checkboxes = new List<CheckboxModel>
                {
                    new CheckboxModel(isChecked:false,value:Placeholders.Label()),
                    new CheckboxModel(isChecked:true,value:Placeholders.Label())
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
