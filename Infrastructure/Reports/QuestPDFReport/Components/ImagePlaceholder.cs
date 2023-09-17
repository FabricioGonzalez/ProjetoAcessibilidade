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

    public float Height
    {
        get;
        set;
    }

    public float Width
    {
        get;
        set;
    }
    public ReportImage(float width, float height)
    {
        Width = width;
        Height = height;
    }
    public void Compose(
        IContainer container
    )
    {
        if (!string.IsNullOrWhiteSpace(ImagePath))
        {
            var image = Image.FromFile(ImagePath);
            container
                .Column(decoration =>
                {
                    decoration
                        .Item()
                        .ExtendHorizontal()
                        .Height(Height)
                        .MaxHeight(Height)
                        .Image(image)
                        .FitUnproportionally()
                        .WithCompressionQuality(ImageCompressionQuality.Medium);

                    decoration
                        .Item()
                        .Border(0.25f)
                        .BorderColor(Colors.Grey.Medium)
                        .Decoration(col =>
                        {
                            col.Before().ExtendHorizontal().AlignCenter().Text("Comentários").Bold();
                            col.Content().ExtendHorizontal().Background(Colors.Grey.Lighten5).Text(Observation);
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