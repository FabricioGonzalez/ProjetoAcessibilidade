using System;
using System.Collections.Generic;
using QuestPDF.Drawing;

namespace QuestPDF.Infrastructure
{
    public abstract class Element : IElement
    {
        public IPageContext PageContext
        {
            get;
            set;
        }

        public ICanvas Canvas
        {
            get;
            set;
        }

        public virtual IEnumerable<Element?> GetChildren()
        {
            yield break;
        }

        public virtual void Initialize(
            IPageContext pageContext
            , ICanvas canvas
        )
        {
            PageContext = pageContext;
            Canvas = canvas;
        }

        public virtual void CreateProxy(
            Func<Element?, Element?> create
        )
        {
        }

        public abstract SpacePlan Measure(
            Size availableSpace
        );

        public abstract void Draw(
            Size availableSpace
        );
    }
}