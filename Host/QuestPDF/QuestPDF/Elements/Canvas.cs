using QuestPDF.Drawing;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.Elements
{
    public delegate void DrawOnCanvas(
        SKCanvas canvas
        , Size availableSpace
    );

    public class Canvas
        : Element
            , ICacheable
    {
        public DrawOnCanvas Handler
        {
            get;
            set;
        }

        public override SpacePlan Measure(
            Size availableSpace
        ) => SpacePlan.FullRender(size: availableSpace);

        public override void Draw(
            Size availableSpace
        )
        {
            var skiaCanvas = (Canvas as SkiaCanvasBase)?.Canvas;

            if (Handler == null || skiaCanvas == null)
            {
                return;
            }

            var originalMatrix = skiaCanvas.TotalMatrix;
            Handler.Invoke(canvas: skiaCanvas, availableSpace: availableSpace);
            skiaCanvas.SetMatrix(matrix: originalMatrix);
        }
    }
}