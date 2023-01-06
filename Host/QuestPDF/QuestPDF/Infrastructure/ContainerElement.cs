using System;
using System.Collections.Generic;
using QuestPDF.Drawing;
using QuestPDF.Elements;

namespace QuestPDF.Infrastructure
{
    public abstract class ContainerElement : Element, IContainer
    {
        public Element? Child { get; set; } = Empty.Instance;

        IElement? IContainer.Child
        {
            get => Child;
            set => Child = value as Element;
        }

        public override IEnumerable<Element?> GetChildren()
        {
            yield return Child;
        }

        public override void CreateProxy(Func<Element?, Element?> create)
        {
            Child = create(Child);
        }

        public override SpacePlan Measure(Size availableSpace)
        {
            return Child?.Measure(availableSpace) ?? SpacePlan.FullRender(0, 0);
        }
        
        public override void Draw(Size availableSpace)
        {
            Child?.Draw(availableSpace);
        }
    }
}