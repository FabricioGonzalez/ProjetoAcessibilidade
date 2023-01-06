using QuestPDF.Elements.Text.Items;

namespace QuestPDF.Elements.Text.Calculation
{
    public class TextLineElement
    {
        public ITextBlockItem Item { get; set; }
        public TextMeasurementResult Measurement { get; set; }
    }
}