using System;
using System.Collections.Generic;
using QuestPDF.Drawing;
using QuestPDF.Elements;

namespace QuestPDF.Infrastructure
{
    public abstract class ContainerElement
        : Element
            , IContainer
    {
        public Element? Child
        {
            get;
            set;
        } = Empty.Instance;

        IElement? IContainer.Child
        {
            get => Child;
            set => Child = value as Element;
        }

        public override IEnumerable<Element?> GetChildren()
        {
            yield return Child;
        }

        public override void CreateProxy(
            Func<Element?, Element?> create
        ) => Child = create(arg: Child);

        public override SpacePlan Measure(
            Size availableSpace
        ) => Child?.Measure(availableSpace: availableSpace) ?? SpacePlan.FullRender(width: 0, height: 0);

        public override void Draw(
            Size availableSpace
        ) => Child?.Draw(availableSpace: availableSpace);
    }
}