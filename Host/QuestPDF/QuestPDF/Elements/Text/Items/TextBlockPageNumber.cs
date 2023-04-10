using System;
using QuestPDF.Elements.Text.Calculation;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements.Text.Items
{
    public class TextBlockPageNumber : TextBlockSpan
    {
        public const string PageNumberPlaceholder = "123";

        public Func<IPageContext, string> Source
        {
            get;
            set;
        } = _ => PageNumberPlaceholder;

        public override TextMeasurementResult? Measure(
            TextMeasurementRequest request
        )
        {
            UpdatePageNumberText(context: request.PageContext);
            return MeasureWithoutCache(request: request);
        }

        public override void Draw(
            TextDrawingRequest request
        )
        {
            UpdatePageNumberText(context: request.PageContext);
            base.Draw(request: request);
        }

        private void UpdatePageNumberText(
            IPageContext context
        ) => Text = Source(arg: context) ?? string.Empty;
    }
}