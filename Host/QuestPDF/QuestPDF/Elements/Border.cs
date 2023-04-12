using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Border : ContainerElement
    {
        public string Color
        {
            get;
            set;
        } = Colors.Black;

        public float Top
        {
            get;
            set;
        }

        public float Right
        {
            get;
            set;
        }

        public float Bottom
        {
            get;
            set;
        }

        public float Left
        {
            get;
            set;
        }

        public override void Draw(
            Size availableSpace
        )
        {
            base.Draw(availableSpace: availableSpace);

            Canvas.DrawRectangle(
                vector: new Position(x: -Left / 2, y: -Top / 2),
                size: new Size(width: availableSpace.Width + Left / 2 + Right / 2, height: Top),
                color: Color);

            Canvas.DrawRectangle(
                vector: new Position(x: -Left / 2, y: -Top / 2),
                size: new Size(width: Left, height: availableSpace.Height + Top / 2 + Bottom / 2),
                color: Color);

            Canvas.DrawRectangle(
                vector: new Position(x: -Left / 2, y: availableSpace.Height - Bottom / 2),
                size: new Size(width: availableSpace.Width + Left / 2 + Right / 2, height: Bottom),
                color: Color);

            Canvas.DrawRectangle(
                vector: new Position(x: availableSpace.Width - Right / 2, y: -Top / 2),
                size: new Size(width: Right, height: availableSpace.Height + Top / 2 + Bottom / 2),
                color: Color);
        }

        public override string ToString() =>
            $"Border: Top({Top}) Right({Right}) Bottom({Bottom}) Left({Left}) Color({Color})";
    }
}