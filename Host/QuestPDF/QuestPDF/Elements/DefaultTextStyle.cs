using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class DefaultTextStyle : ContainerElement
    {
        public TextStyle TextStyle { get; set; } = TextStyle.Default;
    }
}