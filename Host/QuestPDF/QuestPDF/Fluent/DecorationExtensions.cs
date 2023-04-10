using System;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
    public class DecorationDescriptor
    {
        public Decoration Decoration
        {
            get;
        } = new();

        public IContainer Before()
        {
            var container = new Container();
            Decoration.Before = container;
            return container;
        }

        public void Before(
            Action<IContainer> handler
        ) => handler?.Invoke(obj: Before());

        public IContainer Content()
        {
            var container = new Container();
            Decoration.Content = container;
            return container;
        }

        public void Content(
            Action<IContainer> handler
        ) => handler?.Invoke(obj: Content());

        public IContainer After()
        {
            var container = new Container();
            Decoration.After = container;
            return container;
        }

        public void After(
            Action<IContainer> handler
        ) => handler?.Invoke(obj: After());

        #region Obsolete

        [Obsolete(message: "This element has been renamed since version 2022.2. Please use the 'Before' method.")]
        public IContainer Header()
        {
            var container = new Container();
            Decoration.Before = container;
            return container;
        }

        [Obsolete(message: "This element has been renamed since version 2022.2. Please use the 'Before' method.")]
        public void Header(
            Action<IContainer> handler
        ) => handler?.Invoke(obj: Header());

        [Obsolete(message: "This element has been renamed since version 2022.2. Please use the 'After' method.")]
        public IContainer Footer()
        {
            var container = new Container();
            Decoration.After = container;
            return container;
        }

        [Obsolete(message: "This element has been renamed since version 2022.2. Please use the 'After' method.")]
        public void Footer(
            Action<IContainer> handler
        ) => handler?.Invoke(obj: Footer());

        #endregion
    }

    public static class DecorationExtensions
    {
        public static void Decoration(
            this IContainer element
            , Action<DecorationDescriptor> handler
        )
        {
            var descriptor = new DecorationDescriptor();
            handler(obj: descriptor);

            element.Element(child: descriptor.Decoration);
        }
    }
}