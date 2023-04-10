using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDFReport.Components;

public class ImagePlaceholder : IComponent
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
        if (!string.IsNullOrWhiteSpace(value: ImagePath))
        {
            var s = ImagePath;
            using var stream = new FileStream(path: s, mode: FileMode.Open);
            container.Decoration(handler: decoration =>
            {
                decoration
                    .Before()
                    .Height(value: 2f, unit: Unit.Inch)
                    .Border(value: 0.25f)
                    .BorderColor(color: Colors.Grey.Medium)
                    .ScaleToFit()
                    .Image(fileStream: stream, scaling: ImageScaling.FitArea);

                decoration
                    .Content()
                    .Border(value: 0.25f)
                    .BorderColor(color: Colors.Grey.Medium)
                    .Text(text: Observation);
            });
        }
        else
        {
            if (Solid)
            {
                container.Background(color: Placeholders.Color());
            }

            else
            {
                container.Image(imageSource: Placeholders.Image);
            }
        }
    }
}