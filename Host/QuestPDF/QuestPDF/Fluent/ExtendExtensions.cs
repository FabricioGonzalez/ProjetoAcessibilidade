using System;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
    public static class ExtendExtensions
    {
        private static IContainer Extend(
            this IContainer element
            , Action<Extend> handler
        )
        {
            var extend = element as Extend ?? new Extend();
            handler(obj: extend);

            return element.Element(child: extend);
        }

        public static IContainer Extend(
            this IContainer element
        ) => element.ExtendVertical().ExtendHorizontal();

        public static IContainer ExtendVertical(
            this IContainer element
        ) => element.Extend(handler: x => x.ExtendVertical = true);

        public static IContainer ExtendHorizontal(
            this IContainer element
        ) => element.Extend(handler: x => x.ExtendHorizontal = true);
    }
}