using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Rotate : ContainerElement
    {
        public float Angle
        {
            get;
            set;
        } = 0;

        public override void Draw(
            Size availableSpace
        )
        {
            Canvas.Rotate(angle: Angle);
            Child?.Draw(availableSpace: availableSpace);
            Canvas.Rotate(angle: -Angle);
        }
    }
}