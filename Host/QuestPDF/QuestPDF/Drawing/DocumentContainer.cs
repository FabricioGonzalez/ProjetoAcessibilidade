using System;
using System.Collections.Generic;
using System.Linq;
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing
{
    public class DocumentContainer : IDocumentContainer
    {
        public List<IComponent> Pages
        {
            get;
            set;
        } = new();

        public Container Compose()
        {
            var container = new Container();

            container
                .Column(handler: column =>
                {
                    Pages
                        .SelectMany(selector: x => new List<Action>
                        {
                            () => column.Item().PageBreak(), () => column.Item().Component(component: x)
                        })
                        .Skip(count: 1)
                        .ToList()
                        .ForEach(action: x => x());
                });

            return container;
        }
    }
}