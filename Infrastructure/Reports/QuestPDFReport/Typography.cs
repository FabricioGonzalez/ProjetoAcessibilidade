using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDFReport;
public static class Typography
{
    public static TextStyle Title => TextStyle.Default.FontFamily(Fonts.Calibri).FontColor(Colors.Blue.Darken3).FontSize(26).Black();
    public static TextStyle Headline => TextStyle.Default.FontFamily(Fonts.Calibri).FontColor(Colors.Blue.Medium).FontSize(16).SemiBold();
    public static TextStyle SubLine => TextStyle.Default.FontFamily(Fonts.Calibri).FontColor(Colors.Blue.Lighten2).FontSize(12).SemiBold();
    public static TextStyle Normal => TextStyle.Default.FontFamily(Fonts.Calibri).FontColor(Colors.Black).FontSize(10).LineHeight(1.2f);
}
