using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
    public static class LineExtensions
    {
        private static ILine Line(
            this IContainer element
            , LineType type
            , float size
        )
        {
            var line = new Line
            {
                Size = size, Type = type
            };

            element.Element(child: line);
            return line;
        }

        public static ILine LineVertical(
            this IContainer element
            , float size
            , Unit unit = Unit.Point
        ) => element.Line(type: LineType.Vertical, size: size.ToPoints(unit: unit));

        public static ILine LineHorizontal(
            this IContainer element
            , float size
            , Unit unit = Unit.Point
        ) => element.Line(type: LineType.Horizontal, size: size.ToPoints(unit: unit));

        public static void LineColor(
            this ILine descriptor
            , string value
        ) => (descriptor as Line).Color = value;
    }
}