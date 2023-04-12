using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Empty : Element
    {
        public static Empty Instance
        {
            get;
        } = new();

        public override SpacePlan Measure(
            Size availableSpace
        ) => SpacePlan.FullRender(width: 0, height: 0);

        public override void Draw(
            Size availableSpace
        )
        {
        }
    }
}