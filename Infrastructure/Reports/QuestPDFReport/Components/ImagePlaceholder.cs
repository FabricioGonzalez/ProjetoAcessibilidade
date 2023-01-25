﻿using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDFReport.Components;
public class ImagePlaceholder : IComponent
{
    public static bool Solid { get; set; } = false;
    public string ImagePath
    {
        get; set;
    }
    public string Observation
    {
        get; set;
    }

    public void Compose(IContainer container)
    {
        if (!string.IsNullOrWhiteSpace(ImagePath))
        {
            var s = new Uri(ImagePath).AbsolutePath.Replace("%20", " ");
            using var stream = new FileStream(path: s, mode: FileMode.Open);
            container.Decoration(decoration =>
              {
                  decoration
                   .Before()
                   .Height(2f, Unit.Inch)
                   .Border(0.75f)
                   .BorderColor(Colors.Grey.Medium)
                   .Image(stream);

                  decoration
                   .Content()
                   .Border(0.75f)
                   .BorderColor(Colors.Grey.Medium)
                   .Text(Observation);
              });
        }
        else
        {
            if (Solid)
                container.Background(Placeholders.Color());

            else
                container.Image(Placeholders.Image);
        }
    }
}