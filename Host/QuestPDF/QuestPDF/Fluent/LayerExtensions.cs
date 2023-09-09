using System;
using System.Linq;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
    public class LayersDescriptor
    {
        public Layers Layers
        {
            get;
        } = new();

        private IContainer Layer(
            bool isPrimary
        )
        {
            var container = new Container();

            var element = new Layer
            {
                IsPrimary = isPrimary, Child = container
            };

            Layers.Children.Add(item: element);
            return container;
        }

        public IContainer Layer() => Layer(isPrimary: false);
        public IContainer PrimaryLayer() => Layer(isPrimary: true);

        public void Validate()
        {
            var primaryLayers = Layers.Children.Count(predicate: x => x.IsPrimary);

            if (primaryLayers == 0)
            {
                throw new DocumentComposeException(
                    message: "The Layers component needs to have exactly one primary layer. It has none.");
            }

            if (primaryLayers != 1)
            {
                throw new DocumentComposeException(
                    message: $"The Layers component needs to have exactly one primary layer. It has {primaryLayers}.");
            }
        }
    }

    public static class LayerExtensions
    {
        public static void Layers(
            this IContainer element
            , Action<LayersDescriptor> handler
        )
        {
            var descriptor = new LayersDescriptor();

            handler(obj: descriptor);
            descriptor.Validate();

            element.Element(child: descriptor.Layers);
        }
    }
}