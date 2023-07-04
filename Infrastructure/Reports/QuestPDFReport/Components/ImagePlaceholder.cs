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
            using var stream = new FileStream(s, FileMode.Open);
            container.Decoration(decoration =>
            {
                decoration
                    .Before()
                    .Height(2f, Unit.Inch)
                    .Border(0.25f)
                    .BorderColor(Colors.Grey.Medium)
                    .ScaleToFit()
                    .Image(stream, ImageScaling.FitArea);

                decoration
                    .Content()
                    .Border(0.25f)
                    .BorderColor(Colors.Grey.Medium)
                    .Column(col =>
                    {
                        col.Item().LabelCell().AlignCenter().Text("Observações");
                        col.Item().ValueCell().Background(Colors.Grey.Lighten3).Text(Observation);
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

public class CapeImage : IComponent
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

    public void Compose(
        IContainer container
    )
    {
        if (!string.IsNullOrWhiteSpace(ImagePath))
        {
            var s = ImagePath;
            using var stream = new FileStream(s, FileMode.Open);
            container.Decoration(decoration =>
            {
                decoration
                    .Before()
                    .Height(2f, Unit.Inch)
                    .Border(0.25f)
                    .BorderColor(Colors.Grey.Medium)
                    .ScaleToFit()
                    .Image(stream, ImageScaling.FitArea);
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