using System.Collections.Generic;
using System.Linq;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Layer : ContainerElement
    {
        public bool IsPrimary
        {
            get;
            set;
        }
    }

    public class Layers
        : Element
            , ICacheable
    {
        public List<Layer> Children
        {
            get;
            set;
        } = new();

        public override IEnumerable<Element?> GetChildren() => Children;

        public override SpacePlan Measure(
            Size availableSpace
        ) =>
            Children
                .Single(predicate: x => x.IsPrimary)
                .Measure(availableSpace: availableSpace);

        public override void Draw(
            Size availableSpace
        ) =>
            Children
                .Where(predicate: x => x.Measure(availableSpace: availableSpace).Type != SpacePlanType.Wrap)
                .ToList()
                .ForEach(action: x => x.Draw(availableSpace: availableSpace));
    }
}