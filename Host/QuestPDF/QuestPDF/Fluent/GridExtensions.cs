using System;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
    public class GridDescriptor
    {
        public Grid Grid
        {
            get;
        } = new();

        public void Spacing(
            float value
            , Unit unit = Unit.Point
        )
        {
            VerticalSpacing(value: value, unit: unit);
            HorizontalSpacing(value: value, unit: unit);
        }

        public void VerticalSpacing(
            float value
            , Unit unit = Unit.Point
        ) => Grid.VerticalSpacing = value.ToPoints(unit: unit);

        public void HorizontalSpacing(
            float value
            , Unit unit = Unit.Point
        ) => Grid.HorizontalSpacing = value.ToPoints(unit: unit);

        public void Columns(
            int value = Grid.DefaultColumnsCount
        ) => Grid.ColumnsCount = value;

        public void Alignment(
            HorizontalAlignment alignment
        ) => Grid.Alignment = alignment;

        public void AlignLeft() => Alignment(alignment: HorizontalAlignment.Left);
        public void AlignCenter() => Alignment(alignment: HorizontalAlignment.Center);
        public void AlignRight() => Alignment(alignment: HorizontalAlignment.Right);

        public IContainer Item(
            int columns = 1
        )
        {
            var container = new Container();

            var element = new GridElement
            {
                Columns = columns, Child = container
            };

            Grid.Children.Add(item: element);
            return container;
        }
    }

    public static class GridExtensions
    {
        public static void Grid(
            this IContainer element
            , Action<GridDescriptor> handler
        )
        {
            var descriptor = new GridDescriptor();

            if (element is Alignment alignment)
            {
                descriptor.Alignment(alignment: alignment.Horizontal);
            }

            handler(obj: descriptor);
            element.Component(component: descriptor.Grid);
        }
    }
}