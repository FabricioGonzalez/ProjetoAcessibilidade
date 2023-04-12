using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public interface ILine
    {
    }

    public enum LineType
    {
        Vertical
        , Horizontal
    }

    public class Line
        : Element
            , ILine
            , ICacheable
    {
        public LineType Type
        {
            get;
            set;
        } = LineType.Vertical;

        public string Color
        {
            get;
            set;
        } = Colors.Black;

        public float Size
        {
            get;
            set;
        } = 1;

        public override SpacePlan Measure(
            Size availableSpace
        ) =>
            Type switch
            {
                LineType.Vertical when availableSpace.Width + Infrastructure.Size.Epsilon >= Size => SpacePlan
                    .FullRender(width: Size, height: 0)
                , LineType.Horizontal when availableSpace.Height + Infrastructure.Size.Epsilon >= Size => SpacePlan
                    .FullRender(width: 0, height: Size)
                , _ => SpacePlan.Wrap()
            };

        public override void Draw(
            Size availableSpace
        )
        {
            if (Type == LineType.Vertical)
            {
                Canvas.DrawRectangle(vector: new Position(x: -Size / 2, y: 0)
                    , size: new Size(width: Size, height: availableSpace.Height), color: Color);
            }
            else if (Type == LineType.Horizontal)
            {
                Canvas.DrawRectangle(vector: new Position(x: 0, y: -Size / 2)
                    , size: new Size(width: availableSpace.Width, height: Size), color: Color);
            }
        }
    }
}