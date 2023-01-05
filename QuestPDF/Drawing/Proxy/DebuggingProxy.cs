using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing.Proxy
{
    public class DebuggingProxy : ElementProxy
    {
        private DebuggingState DebuggingState { get; }

        public DebuggingProxy(DebuggingState debuggingState, Element child)
        {
            DebuggingState = debuggingState;
            Child = child;
        }
        
        public override SpacePlan Measure(Size availableSpace)
        {
            DebuggingState.RegisterMeasure(Child, availableSpace);
            var spacePlan = base.Measure(availableSpace);
            DebuggingState.RegisterMeasureResult(Child, spacePlan);

            return spacePlan;
        }
    }
}