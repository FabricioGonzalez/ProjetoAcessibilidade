using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Background : ContainerElement
    {
        public string Color { get; set; } = Colors.Black;
        
        public override void Draw(Size availableSpace)
        {
            Canvas.DrawRectangle(Position.Zero, availableSpace, Color);
            base.Draw(availableSpace);
        }
    }
}