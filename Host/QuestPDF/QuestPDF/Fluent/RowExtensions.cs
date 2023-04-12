using System;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
    public class RowDescriptor
    {
        public Row Row
        {
            get;
        } = new();

        public void Spacing(
            float value
        ) => Row.Spacing = value;

        private IContainer Item(
            RowItemType type
            , float size = 0
        )
        {
            var element = new RowItem
            {
                Type = type, Size = size
            };

            Row.Items.Add(item: element);
            return element;
        }

        [Obsolete(message: "This element has been renamed since version 2022.2. Please use the RelativeItem method.")]
        public IContainer RelativeColumn(
            float size = 1
        ) => Item(type: RowItemType.Relative, size: size);

        [Obsolete(message: "This element has been renamed since version 2022.2. Please use the ConstantItem method.")]
        public IContainer ConstantColumn(
            float size
        ) => Item(type: RowItemType.Constant, size: size);

        public IContainer RelativeItem(
            float size = 1
        ) => Item(type: RowItemType.Relative, size: size);

        public IContainer ConstantItem(
            float size
            , Unit unit = Unit.Point
        ) => Item(type: RowItemType.Constant, size: size.ToPoints(unit: unit));

        public IContainer AutoItem() => Item(type: RowItemType.Auto);
    }

    public static class RowExtensions
    {
        public static void Row(
            this IContainer element
            , Action<RowDescriptor> handler
        )
        {
            var descriptor = new RowDescriptor();
            handler(obj: descriptor);
            element.Element(child: descriptor.Row);
        }
    }
}