using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Translate : ContainerElement
    {
        public float TranslateX
        {
            get;
            set;
        } = 0;

        public float TranslateY
        {
            get;
            set;
        } = 0;

        public override void Draw(
            Size availableSpace
        )
        {
            var translate = new Position(x: TranslateX, y: TranslateY);

            Canvas.Translate(vector: translate);
            base.Draw(availableSpace: availableSpace);
            Canvas.Translate(vector: translate.Reverse());
        }
    }
}