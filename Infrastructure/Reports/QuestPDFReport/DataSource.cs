using App.Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using App.Core.Entities.Solution.Project.AppItem.DataItems.Text;

using Common;

using ProjectItemReader.XmlFile;

using QuestPDF.Helpers;

using QuestPDFReport.Models;

namespace QuestPDFReport;
public static class DataSource
{
    public static async Task<ReportModel> GetReport(string path)
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

        var items = Directory.GetFiles(res, $"*.{Constants.AppProjectItemExtension}");

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
                    section.Parts.Add(new ReportSectionTitle(checkbox.Topic is not null ? checkbox.Topic : ""));

                    foreach (var options in checkbox.Children)
                    {
                        var checkboxModels = options.Options.Select(item => new CheckboxModel(isChecked: item.IsChecked, value: item.Value));

                        var reportCheckbox = new ReportSectionCheckbox() { Label = options.Topic, Checkboxes = checkboxModels.ToList() };

                        section.Parts.Add(reportCheckbox);
                    }
                }
            }

            report.Sections.Add(section);
        }

        return report;
    }
    public static ReportModel GetReport()
    {
        return new ReportModel
        {
            Title = "Sample Report Document",
            HeaderFields = HeaderFields(),

            LogoData = Helpers.GetImage("Logo.png"),
            Sections = Enumerable.Range(0, 40).Select(x => GenerateSection()).ToList(),
            Photos = Enumerable.Range(0, 25).Select(x => GetReportPhotos()).ToList()
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

        ReportSectionPhotos GetPhotosElement()
        {
            return new ReportSectionPhotos
            {
                Label = "Photos",
                PhotoCount = Helpers.Random.Next(1, 15)
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
