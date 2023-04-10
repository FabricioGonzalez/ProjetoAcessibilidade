using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDFReport.Models;

namespace QuestPDFReport.Components;

public class PhotoTemplate : IComponent
{
    public PhotoTemplate(
        ReportPhoto model
    )
    {
        Model = model;
    }

    public ReportPhoto Model
    {
        get;
        set;
    }

    public void Compose(
        IContainer container
    ) =>
        container
            .ShowEntire()
            .Column(handler: column =>
            {
                column.Spacing(value: 5);
                column.Item().Element(handler: PhotoWithMaps);
                column.Item().Element(handler: PhotoDetails);
            });

    private void PhotoWithMaps(
        IContainer container
    ) =>
        container
            .Row(handler: row =>
            {
                row.RelativeItem(size: 2).AspectRatio(ratio: 4 / 3f).Component<ImagePlaceholder>();

                row.RelativeItem().PaddingLeft(value: 5).Column(handler: column =>
                {
                    column.Spacing(value: 7f);

                    column.Item().AspectRatio(ratio: 4 / 3f).Component<ImagePlaceholder>();
                    column.Item().AspectRatio(ratio: 4 / 3f).Component<ImagePlaceholder>();
                });
            });

    private void PhotoDetails(
        IContainer container
    ) =>
        container
            .Border(value: 0.75f)
            .BorderColor(color: Colors.Grey.Medium)
            .Grid(handler: grid =>
            {
                grid.Columns(value: 6);

                grid.Item().LabelCell().Text(text: "Date");
                grid.Item(columns: 2).ValueCell().Text(text: Model.Date?.ToString(format: "g") ?? string.Empty);
                grid.Item().LabelCell().Text(text: "Location");
                grid.Item(columns: 2).ValueCell().Text(text: Model.Location.Format());

                grid.Item().LabelCell().Text(text: "Comments");
                grid.Item(columns: 5).ValueCell().Text(text: Model.Comments);
            });
}