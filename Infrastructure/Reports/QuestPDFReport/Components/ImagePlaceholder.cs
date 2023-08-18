using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDFReport.Components;

public class ReportImage : IComponent
{
    public static bool Solid
    {
        get;
        set;
    } = false;

    public string ImagePath
    {
        get;
        set;
    }

    public string Observation
    {
        get;
        set;
    }

    public void Compose(
        IContainer container
    )
    {
        if (!string.IsNullOrWhiteSpace(ImagePath))
        {
            var s = ImagePath;
            using var stream = new FileStream(path: s, mode: FileMode.Open);
            container.Height(value: 1, unit: Unit.Inch).Decoration(decoration =>
            {
                decoration
                    .Before()
                    .Border(0.25f)
                    .BorderColor(Colors.Grey.Medium)
                    .ExtendHorizontal()
                    .Image(fileStream: stream, scaling: ImageScaling.FitArea);

                decoration
                    .Content()
                    .Border(0.25f)
                    .BorderColor(Colors.Grey.Medium)
                    .Column(col =>
                    {
                        col.Item().LabelCell().AlignCenter().Text("Comentários");
                        col.Item().ValueCell().Background(Colors.Grey.Lighten1).Text(Observation);
                    });
            });
        }
        else
        {
            if (Solid)
            {
                container.Background(Placeholders.Color());
            }

            else
            {
                container.Image(Placeholders.Image);
            }
        }
    }
}