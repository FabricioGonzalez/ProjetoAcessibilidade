using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDFReport.Models;

namespace QuestPDFReport.Components;

public static class Helpers
{
    private static IContainer Cell(
        this IContainer container
        , bool background
    ) =>
        container
            .Border(value: 0.5f)
            .BorderColor(color: Colors.Grey.Lighten1)
            .Background(color: background ? Colors.Grey.Lighten4 : Colors.White)
            .Padding(value: 5);

    public static IContainer ValueCell(
        this IContainer container
    ) => container.Cell(background: false);

    public static IContainer LabelCell(
        this IContainer container
    ) => container.Cell(background: true);

    public static string Format(
        this Location location
    )
    {
        if (location == null)
        {
            return string.Empty;
        }

        var lon = location.Longitude;
        var lat = location.Latitude;

        var typeLon = lon > 0 ? "E" : "W";
        lon = Math.Abs(value: lon);

        var typeLat = lat > 0 ? "N" : "S";
        lat = Math.Abs(value: lat);

        return $"{lat:F5}° {typeLat}   {lon:F5}° {typeLon}";
    }
}