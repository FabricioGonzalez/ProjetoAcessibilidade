using System;
using System.Collections.Generic;
using System.Linq;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class ColumnItem : Container
    {
        public bool IsRendered
        {
            get;
            set;
        }
    }

    public class ColumnItemRenderingCommand
    {
        public ColumnItem ColumnItem
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

    public class Column
        : Element
            , ICacheable
            , IStateResettable
    {
        public List<ColumnItem> Items
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

            var renderingCommands = PlanLayout(availableSpace: availableSpace);

            if (!renderingCommands.Any())
            {
                return SpacePlan.Wrap();
            }

            var width = renderingCommands.Max(selector: x => x.Size.Width);
            var height = renderingCommands.Last().Offset.Y + renderingCommands.Last().Size.Height;
            var size = new Size(width: width, height: height);

            if (width > availableSpace.Width + Size.Epsilon || height > availableSpace.Height + Size.Epsilon)
            {
                return SpacePlan.Wrap();
            }

            var totalRenderedItems = Items.Count(predicate: x => x.IsRendered) +
                                     renderingCommands.Count(predicate: x =>
                                         x.Measurement.Type == SpacePlanType.FullRender);
            var willBeFullyRendered = totalRenderedItems == Items.Count;

            return willBeFullyRendered
                ? SpacePlan.FullRender(size: size)
                : SpacePlan.PartialRender(size: size);
        }

        public override void Draw(
            Size availableSpace
        )
        {
            var renderingCommands = PlanLayout(availableSpace: availableSpace);

            foreach (var command in renderingCommands)
            {
                if (command.Measurement.Type == SpacePlanType.FullRender)
                {
                    command.ColumnItem.IsRendered = true;
                }

                var targetSize = new Size(width: availableSpace.Width, height: command.Size.Height);

                Canvas.Translate(vector: command.Offset);
                command.ColumnItem.Draw(availableSpace: targetSize);
                Canvas.Translate(vector: command.Offset.Reverse());
            }

            if (Items.All(predicate: x => x.IsRendered))
            {
                ResetState();
            }
        }

        private ICollection<ColumnItemRenderingCommand> PlanLayout(
            Size availableSpace
        )
        {
            var topOffset = 0f;
            var commands = new List<ColumnItemRenderingCommand>();

            foreach (var item in Items)
            {
                if (item.IsRendered)
                {
                    continue;
                }

                var itemSpace = new Size(width: availableSpace.Width, height: availableSpace.Height - topOffset);
                var measurement = item.Measure(availableSpace: itemSpace);

                if (measurement.Type == SpacePlanType.Wrap)
                {
                    break;
                }

                commands.Add(item: new ColumnItemRenderingCommand
                {
                    ColumnItem = item, Size = measurement, Measurement = measurement
                    , Offset = new Position(x: 0, y: topOffset)
                });

                if (measurement.Type == SpacePlanType.PartialRender)
                {
                    break;
                }

                topOffset += measurement.Height + Spacing;
            }

            var targetWidth = commands.Select(selector: x => x.Size.Width).DefaultIfEmpty(defaultValue: 0).Max();
            commands.ForEach(action: x => x.Size = new Size(width: targetWidth, height: x.Size.Height));

            return commands;
        }
    }
}