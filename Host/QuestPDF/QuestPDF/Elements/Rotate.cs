using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Rotate : ContainerElement
    {
        public float Angle { get; set; } = 0;

        public override void Draw(Size availableSpace)
        {
            Canvas.Rotate(Angle);
            Child?.Draw(availableSpace);
            Canvas.Rotate(-Angle);
        }
    }
}