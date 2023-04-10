using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing.Proxy
{
    public class DebuggingProxy : ElementProxy
    {
        public DebuggingProxy(
            DebuggingState debuggingState
            , Element child
        )
        {
            DebuggingState = debuggingState;
            Child = child;
        }

        private DebuggingState DebuggingState
        {
            get;
        }

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            DebuggingState.RegisterMeasure(element: Child, availableSpace: availableSpace);
            var spacePlan = base.Measure(availableSpace: availableSpace);
            DebuggingState.RegisterMeasureResult(element: Child, spacePlan: spacePlan);

            return spacePlan;
        }
    }
}