using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Empty : Element
    {
        public static Empty Instance { get; } = new Empty();
        
        public override SpacePlan Measure(Size availableSpace)
        {
            return SpacePlan.FullRender(0, 0);
        }

        public override void Draw(Size availableSpace)
        {
            
        }
    }
}