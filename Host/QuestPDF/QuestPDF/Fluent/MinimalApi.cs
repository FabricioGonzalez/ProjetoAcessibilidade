using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
    public class Document : IDocument
    {
        private Document(
            Action<IDocumentContainer> contentSource
        )
        {
            ContentSource = contentSource;
        }

        private Action<IDocumentContainer> ContentSource
        {
            get;
        }

        private DocumentMetadata Metadata
        {
            get;
            set;
        } = DocumentMetadata.Default;

        public static Document Create(
            Action<IDocumentContainer> handler
        ) => new(contentSource: handler);

        public Document WithMetadata(
            DocumentMetadata metadata
        )
        {
            Metadata = metadata ?? Metadata;
            return this;
        }

        #region IDocument

        public DocumentMetadata GetMetadata() => Metadata;

        public void Compose(
            IDocumentContainer container
        ) => ContentSource(obj: container);

        #endregion
    }
}