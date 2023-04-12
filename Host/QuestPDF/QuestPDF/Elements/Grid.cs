using System.Collections.Generic;
using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class GridElement
    {
        public int Columns
        {
            get;
            set;
        } = 1;

        public Element? Child
        {
            get;
            set;
        }
    }

    public class Grid : IComponent
    {
        public const int DefaultColumnsCount = 12;

        public List<GridElement> Children
        {
            get;
        } = new();

        public Queue<GridElement> ChildrenQueue
        {
            get;
            set;
        } = new();

        public int ColumnsCount
        {
            get;
            set;
        } = DefaultColumnsCount;

        public HorizontalAlignment Alignment
        {
            get;
            set;
        } = HorizontalAlignment.Left;

        public float VerticalSpacing
        {
            get;
            set;
        } = 0;

        public float HorizontalSpacing
        {
            get;
            set;
        } = 0;

        public void Compose(
            IContainer container
        )
        {
            ChildrenQueue = new Queue<GridElement>(collection: Children);

            container.Column(handler: column =>
            {
                column.Spacing(value: VerticalSpacing);

                while (ChildrenQueue.Any())
                {
                    column.Item().Row(handler: BuildRow);
                }
            });
        }

        private IEnumerable<GridElement> GetRowElements()
        {
            var rowLength = 0;

            while (ChildrenQueue.Any())
            {
                var element = ChildrenQueue.Peek();

                if (rowLength + element.Columns > ColumnsCount)
                {
                    break;
                }

                rowLength += element.Columns;
                yield return ChildrenQueue.Dequeue();
            }
        }

        private void BuildRow(
            RowDescriptor row
        )
        {
            row.Spacing(value: HorizontalSpacing);

            var elements = GetRowElements().ToList();
            var columnsWidth = elements.Sum(selector: x => x.Columns);
            var emptySpace = ColumnsCount - columnsWidth;
            var hasEmptySpace = emptySpace >= Size.Epsilon;

            if (Alignment == HorizontalAlignment.Center)
            {
                emptySpace /= 2;
            }

            if (hasEmptySpace && Alignment != HorizontalAlignment.Left)
            {
                row.RelativeItem(size: emptySpace);
            }

            elements.ForEach(action: x => row.RelativeItem(size: x.Columns).Element(child: x.Child));

            if (hasEmptySpace && Alignment != HorizontalAlignment.Right)
            {
                row.RelativeItem(size: emptySpace);
            }
        }
    }
}