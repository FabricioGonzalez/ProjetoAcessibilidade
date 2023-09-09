using System;
using System.Collections.Generic;
using System.IO;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Drawing.Proxy;
using QuestPDF.Elements;
using QuestPDF.Elements.Text;
using QuestPDF.Elements.Text.Items;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing
{
    public static class DocumentGenerator
    {
        public static void GeneratePdf(
            Stream stream
            , IDocument document
        )
        {
            CheckIfStreamIsCompatible(stream: stream);

            var metadata = document.GetMetadata();
            var canvas = new PdfCanvas(stream: stream, documentMetadata: metadata);
            RenderDocument(canvas: canvas, document: document);
        }

        public static void GenerateXps(
            Stream stream
            , IDocument document
        )
        {
            CheckIfStreamIsCompatible(stream: stream);

            var metadata = document.GetMetadata();
            var canvas = new XpsCanvas(stream: stream, documentMetadata: metadata);
            RenderDocument(canvas: canvas, document: document);
        }

        internal static ICollection<PreviewerPicture> GeneratePreviewerPictures(
            IDocument document
        )
        {
            var canvas = new SkiaPictureCanvas();
            RenderDocument(canvas: canvas, document: document);
            return canvas.Pictures;
        }

        private static void CheckIfStreamIsCompatible(
            Stream stream
        )
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException(
                    message:
                    "The library requires a Stream object with the 'write' capability available (the CanWrite flag). Please consider using the MemoryStream class.");
            }

            if (!stream.CanSeek)
            {
                throw new ArgumentException(
                    message:
                    "The library requires a Stream object with the 'seek' capability available (the CanSeek flag). Please consider using the MemoryStream class.");
            }
        }

        public static ICollection<byte[]> GenerateImages(
            IDocument document
        )
        {
            var metadata = document.GetMetadata();
            var canvas = new ImageCanvas(metadata: metadata);
            RenderDocument(canvas: canvas, document: document);

            return canvas.Images;
        }

        public static void RenderDocument<TCanvas>(
            TCanvas canvas
            , IDocument document
        )
            where TCanvas : ICanvas, IRenderingCanvas
        {
            var container = new DocumentContainer();
            document.Compose(container: container);
            var content = container.Compose();
            ApplyDefaultTextStyle(content: content, documentDefaultTextStyle: TextStyle.LibraryDefault);

            var metadata = document.GetMetadata();
            var pageContext = new PageContext();

            var debuggingState = metadata.ApplyDebugging ? ApplyDebugging(content: content) : null;

            if (metadata.ApplyCaching)
            {
                ApplyCaching(content: content);
            }

            RenderPass(pageContext: pageContext, canvas: new FreeCanvas(), content: content, documentMetadata: metadata
                , debuggingState: debuggingState);
            RenderPass(pageContext: pageContext, canvas: canvas, content: content, documentMetadata: metadata
                , debuggingState: debuggingState);
        }

        public static void RenderPass<TCanvas>(
            PageContext pageContext
            , TCanvas canvas
            , Container content
            , DocumentMetadata documentMetadata
            , DebuggingState? debuggingState
        )
            where TCanvas : ICanvas, IRenderingCanvas
        {
            content.VisitChildren(handler: x => x?.Initialize(pageContext: pageContext, canvas: canvas));
            content.VisitChildren(handler: x => (x as IStateResettable)?.ResetState());

            canvas.BeginDocument();

            var currentPage = 1;

            while (true)
            {
                pageContext.SetPageNumber(number: currentPage);
                debuggingState?.Reset();

                var spacePlan = content.Measure(availableSpace: Size.Max);

                if (spacePlan.Type == SpacePlanType.Wrap)
                {
                    canvas.EndDocument();
                    ThrowLayoutException();
                }

                try
                {
                    canvas.BeginPage(size: spacePlan);
                    content.Draw(availableSpace: spacePlan);
                }
                catch (Exception exception)
                {
                    canvas.EndDocument();
                    throw new DocumentDrawingException(message: "An exception occured during document drawing."
                        , inner: exception);
                }

                canvas.EndPage();

                if (currentPage >= documentMetadata.DocumentLayoutExceptionThreshold)
                {
                    canvas.EndDocument();
                    ThrowLayoutException();
                }

                if (spacePlan.Type == SpacePlanType.FullRender)
                {
                    break;
                }

                currentPage++;
            }

            canvas.EndDocument();

            void ThrowLayoutException()
            {
                var message = "Composed layout generates infinite document. This may happen in two cases. " +
                              $"1) Your document and its layout configuration is correct but the content takes more than {documentMetadata.DocumentLayoutExceptionThreshold} pages. " +
                              $"In this case, please increase the value {nameof(DocumentMetadata)}.{nameof(DocumentMetadata.DocumentLayoutExceptionThreshold)} property configured in the {nameof(IDocument.GetMetadata)} method. " +
                              "2) The layout configuration of your document is invalid. Some of the elements require more space than is provided." +
                              "Please analyze your documents structure to detect this element and fix its size constraints.";

                throw new DocumentLayoutException(message: message)
                {
                    ElementTrace = debuggingState?.BuildTrace() ?? "Debug trace is available only in the DEBUG mode."
                };
            }
        }

        private static void ApplyCaching(
            Container content
        ) =>
            content.VisitChildren(handler: x =>
            {
                if (x is ICacheable)
                {
                    x.CreateProxy(create: y => new CacheProxy(child: y));
                }
            });

        private static DebuggingState ApplyDebugging(
            Container content
        )
        {
            var debuggingState = new DebuggingState();

            content.VisitChildren(handler: x =>
            {
                x.CreateProxy(create: y => new DebuggingProxy(debuggingState: debuggingState, child: y));
            });

            return debuggingState;
        }

        private static void ApplyDefaultTextStyle(
            this Element? content
            , TextStyle documentDefaultTextStyle
        )
        {
            if (content == null)
            {
                return;
            }

            if (content is TextBlock textBlock)
            {
                foreach (var textBlockItem in textBlock.Items)
                {
                    if (textBlockItem is TextBlockSpan textSpan)
                    {
                        textSpan.Style.ApplyGlobalStyle(globalStyle: documentDefaultTextStyle);
                    }
                    else if (textBlockItem is TextBlockElement textElement)
                    {
                        ApplyDefaultTextStyle(content: textElement.Element
                            , documentDefaultTextStyle: documentDefaultTextStyle);
                    }
                }

                return;
            }

            var targetTextStyle = documentDefaultTextStyle;

            if (content is DefaultTextStyle defaultTextStyleElement)
            {
                defaultTextStyleElement.TextStyle.ApplyParentStyle(parentStyle: documentDefaultTextStyle);
                targetTextStyle = defaultTextStyleElement.TextStyle;
            }

            foreach (var child in content.GetChildren())
            {
                ApplyDefaultTextStyle(content: child, documentDefaultTextStyle: targetTextStyle);
            }
        }
    }
}