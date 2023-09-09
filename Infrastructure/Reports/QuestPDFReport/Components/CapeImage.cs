using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDFReport.Components;

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

    public float Width
    {
        get;
        set;
    } = 20;

    public float Height
    {
        get;
        set;
    } = 20;

    public void Compose(
        IContainer container
    )
    {
        if (!string.IsNullOrWhiteSpace(ImagePath))
        {
            var s = ImagePath;
            using var stream = new FileStream(path: s, mode: FileMode.Open);

            container
                .ScaleToFit()
                .Image(fileStream: stream, scaling: ImageScaling.FitArea);
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