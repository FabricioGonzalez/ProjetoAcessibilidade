using System;
using System.Collections.Generic;
using System.Linq;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Layer : ContainerElement
    {
        public bool IsPrimary { get; set; }
    }
    
    public class Layers : Element, ICacheable
    {
        public List<Layer> Children { get; set; } = new List<Layer>();
        
        public override IEnumerable<Element?> GetChildren()
        {
            return Children;
        }
        
        public override SpacePlan Measure(Size availableSpace)
        {
            return Children
                .Single(x => x.IsPrimary)
                .Measure(availableSpace);
        }

        public override void Draw(Size availableSpace)
        {
            Children
                .Where(x => x.Measure(availableSpace).Type != SpacePlanType.Wrap)
                .ToList()
                .ForEach(x => x.Draw(availableSpace));
        }
    }
}