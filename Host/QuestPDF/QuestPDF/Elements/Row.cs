using System;
using System.Collections.Generic;
using System.Linq;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public enum RowItemType
    {
        Auto
        , Constant
        , Relative
    }

    public class RowItem : Container
    {
        public bool IsRendered
        {
            get;
            set;
        }

        public float Width
        {
            get;
            set;
        }

        public RowItemType Type
        {
            get;
            set;
        }

        public float Size
        {
            get;
            set;
        }
    }

    public class RowItemRenderingCommand
    {
        public RowItem RowItem
        {
            get;
            set;
        }

        public SpacePlan Measurement
        {
            get;
            set;
        }

        public Size Size
        {
            get;
            set;
        }

        public Position Offset
        {
            get;
            set;
        }
    }

    public class Row
        : Element
            , ICacheable
            , IStateResettable
    {
        public List<RowItem> Items
        {
            get;
        } = new();

        public float Spacing
        {
            get;
            set;
        }

        public void ResetState() => Items.ForEach(action: x => x.IsRendered = false);

        public override IEnumerable<Element?> GetChildren() => Items;

        public override void CreateProxy(
            Func<Element?, Element?> create
        ) => Items.ForEach(action: x => x.Child = create(arg: x.Child));

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            if (!Items.Any())
            {
                return SpacePlan.FullRender(size: Size.Zero);
            }

            UpdateItemsWidth(availableWidth: availableSpace.Width);
            var renderingCommands = PlanLayout(availableSpace: availableSpace);

            if (renderingCommands.Any(predicate: x =>
                    !x.RowItem.IsRendered && x.Measurement.Type == SpacePlanType.Wrap))
            {
                return SpacePlan.Wrap();
            }

            var width = renderingCommands.Last().Offset.X + renderingCommands.Last().Size.Width;
            var height = renderingCommands.Max(selector: x => x.Size.Height);
            var size = new Size(width: width, height: height);

            if (width > availableSpace.Width + Size.Epsilon || height > availableSpace.Height + Size.Epsilon)
            {
                return SpacePlan.Wrap();
            }

            if (renderingCommands.Any(predicate: x =>
                    !x.RowItem.IsRendered && x.Measurement.Type == SpacePlanType.PartialRender))
            {
                return SpacePlan.PartialRender(size: size);
            }

            return SpacePlan.FullRender(size: size);
        }

        public override void Draw(
            Size availableSpace
        )
        {
            if (!Items.Any())
            {
                return;
            }

            UpdateItemsWidth(availableWidth: availableSpace.Width);
            var renderingCommands = PlanLayout(availableSpace: availableSpace);

            foreach (var command in renderingCommands)
            {
                if (command.Measurement.Type == SpacePlanType.FullRender)
                {
                    command.RowItem.IsRendered = true;
                }

                if (command.Measurement.Type == SpacePlanType.Wrap)
                {
                    continue;
                }

                Canvas.Translate(vector: command.Offset);
                command.RowItem.Draw(availableSpace: command.Size);
                Canvas.Translate(vector: command.Offset.Reverse());
            }

            if (Items.All(predicate: x => x.IsRendered))
            {
                ResetState();
            }
        }

        private void UpdateItemsWidth(
            float availableWidth
        )
        {
            HandleItemsWithAutoWidth();

            var constantWidth = Items.Where(predicate: x => x.Type == RowItemType.Constant).Sum(selector: x => x.Size);
            var relativeWidth = Items.Where(predicate: x => x.Type == RowItemType.Relative).Sum(selector: x => x.Size);
            var spacingWidth = (Items.Count - 1) * Spacing;

            foreach (var item in Items.Where(predicate: x => x.Type == RowItemType.Constant))
            {
                item.Width = item.Size;
            }

            if (relativeWidth <= 0)
            {
                return;
            }

            var widthPerRelativeUnit = (availableWidth - constantWidth - spacingWidth) / relativeWidth;

            foreach (var item in Items.Where(predicate: x => x.Type == RowItemType.Relative))
            {
                item.Width = item.Size * widthPerRelativeUnit;
            }
        }

        private void HandleItemsWithAutoWidth()
        {
            foreach (var rowItem in Items.Where(predicate: x => x.Type == RowItemType.Auto))
            {
                rowItem.Size = rowItem.Measure(availableSpace: Size.Max).Width;
                rowItem.Type = RowItemType.Constant;
            }
        }

        private ICollection<RowItemRenderingCommand> PlanLayout(
            Size availableSpace
        )
        {
            var leftOffset = 0f;
            var renderingCommands = new List<RowItemRenderingCommand>();

            foreach (var item in Items)
            {
                var itemSpace = new Size(width: item.Width, height: availableSpace.Height);

                var command = new RowItemRenderingCommand
                {
                    RowItem = item, Size = itemSpace, Measurement = item.Measure(availableSpace: itemSpace)
                    , Offset = new Position(x: leftOffset, y: 0)
                };

                renderingCommands.Add(item: command);
                leftOffset += item.Width + Spacing;
            }

            var rowHeight = renderingCommands
                .Where(predicate: x => !x.RowItem.IsRendered)
                .Select(selector: x => x.Measurement.Height)
                .DefaultIfEmpty(defaultValue: 0)
                .Max();

            foreach (var command in renderingCommands)
            {
                command.Size = new Size(width: command.Size.Width, height: rowHeight);
                command.Measurement = command.RowItem.Measure(availableSpace: command.Size);
            }

            return renderingCommands;
        }
    }
}