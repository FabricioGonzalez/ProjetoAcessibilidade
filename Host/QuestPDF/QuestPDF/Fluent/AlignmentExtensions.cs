using System;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
    public static class AlignmentExtensions
    {
        private static IContainer Alignment(
            this IContainer element
            , Action<Alignment> handler
        )
        {
            var alignment = element as Alignment ?? new Alignment();
            handler(obj: alignment);

            return element.Element(child: alignment);
        }

        public static IContainer AlignLeft(
            this IContainer element
        ) => element.Alignment(handler: x => x.Horizontal = HorizontalAlignment.Left);

        public static IContainer AlignCenter(
            this IContainer element
        ) => element.Alignment(handler: x => x.Horizontal = HorizontalAlignment.Center);

        public static IContainer AlignRight(
            this IContainer element
        ) => element.Alignment(handler: x => x.Horizontal = HorizontalAlignment.Right);

        public static IContainer AlignTop(
            this IContainer element
        ) => element.Alignment(handler: x => x.Vertical = VerticalAlignment.Top);

        public static IContainer AlignMiddle(
            this IContainer element
        ) => element.Alignment(handler: x => x.Vertical = VerticalAlignment.Middle);

        public static IContainer AlignBottom(
            this IContainer element
        ) => element.Alignment(handler: x => x.Vertical = VerticalAlignment.Bottom);
    }
}