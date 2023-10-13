using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDFReport.Components;

public class ReportImage : IComponent
{
    private readonly bool strecth;

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
    public ReportImage(float width, float height, bool strecth = false)
    {
        Width = width;
        Height = height;
        this.strecth = strecth;
    }
    public void Compose(
        IContainer container
    )
    {
        if (!string.IsNullOrWhiteSpace(ImagePath))
        {
            container
                .Column(decoration =>
                {
                    if (File.Exists(ImagePath))
                    {
                        var image = Image.FromFile(ImagePath);
                        if (strecth)
                        {
                            decoration
                             .Item()
                             .ExtendHorizontal()
                             .Height(Height)
                             .MaxHeight(Height)
                             .Image(image)
                             .WithCompressionQuality(ImageCompressionQuality.Medium)
                             .FitUnproportionally();
                        }
                        else
                        {
                            decoration
                             .Item()
                             .ExtendHorizontal()
                             .AlignCenter()
                             .Height(Height)
                             .MaxHeight(Height)
                             .Image(image)
                             .WithCompressionQuality(ImageCompressionQuality.Medium)
                             .FitArea();
                        }
                    }
                    else
                    {
                        decoration
                             .Item()
                             .ExtendHorizontal()
                             .AlignCenter()
                             .AlignMiddle()
                             .Height(Height)
                             .MaxHeight(Height)
                             .Text("Imagem não Encontrada")
                             .Style(Typography.Title);

                    }


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