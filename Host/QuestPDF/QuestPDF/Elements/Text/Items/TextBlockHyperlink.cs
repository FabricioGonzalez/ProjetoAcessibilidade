using QuestPDF.Elements.Text.Calculation;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements.Text.Items
{
    public class TextBlockHyperlink : TextBlockSpan
    {
        public string Url
        {
            get;
            set;
        }

        public override TextMeasurementResult? Measure(
            TextMeasurementRequest request
        ) => MeasureWithoutCache(request: request);

        public override void Draw(
            TextDrawingRequest request
        )
        {
            request.Canvas.Translate(vector: new Position(x: 0, y: request.TotalAscent));
            request.Canvas.DrawHyperlink(url: Url
                , size: new Size(width: request.TextSize.Width, height: request.TextSize.Height));
            request.Canvas.Translate(vector: new Position(x: 0, y: -request.TotalAscent));

            base.Draw(request: request);
        }
    }
}