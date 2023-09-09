using QuestPDF.Drawing;
using QuestPDF.Elements.Text.Calculation;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements.Text.Items
{
    public class TextBlockElement : ITextBlockItem
    {
        public Element Element
        {
            get;
            set;
        } = Empty.Instance;

        public TextMeasurementResult? Measure(
            TextMeasurementRequest request
        )
        {
            Element.VisitChildren(handler: x => (x as IStateResettable)?.ResetState());
            Element.VisitChildren(handler: x => x.Initialize(pageContext: request.PageContext, canvas: request.Canvas));

            var measurement =
                Element.Measure(availableSpace: new Size(width: request.AvailableWidth, height: Size.Max.Height));

            if (measurement.Type != SpacePlanType.FullRender)
            {
                return null;
            }

            return new TextMeasurementResult
            {
                Width = measurement.Width, Ascent = -measurement.Height, Descent = 0, LineHeight = 1, StartIndex = 0
                , EndIndex = 0, TotalIndex = 0
            };
        }

        public void Draw(
            TextDrawingRequest request
        )
        {
            Element.VisitChildren(handler: x => (x as IStateResettable)?.ResetState());
            Element.VisitChildren(handler: x => x.Initialize(pageContext: request.PageContext, canvas: request.Canvas));

            request.Canvas.Translate(vector: new Position(x: 0, y: request.TotalAscent));
            Element.Draw(availableSpace: new Size(width: request.TextSize.Width, height: -request.TotalAscent));
            request.Canvas.Translate(vector: new Position(x: 0, y: -request.TotalAscent));
        }
    }
}