using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Background : ContainerElement
    {
        public string Color
        {
            get;
            set;
        } = Colors.Black;

        public override void Draw(
            Size availableSpace
        )
        {
            Canvas.DrawRectangle(vector: Position.Zero, size: availableSpace, color: Color);
            base.Draw(availableSpace: availableSpace);
        }
    }
}