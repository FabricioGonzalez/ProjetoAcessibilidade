using System;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing.Proxy
{
    public class CacheProxy : ElementProxy
    {
        public CacheProxy(
            Element child
        )
        {
            Child = child;
        }

        public Size? AvailableSpace
        {
            get;
            set;
        }

        public SpacePlan? MeasurementResult
        {
            get;
            set;
        }

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            if (MeasurementResult != null &&
                AvailableSpace != null &&
                IsClose(x: AvailableSpace.Value.Width, y: availableSpace.Width) &&
                IsClose(x: AvailableSpace.Value.Height, y: availableSpace.Height))
            {
                return MeasurementResult.Value;
            }

            AvailableSpace = availableSpace;
            MeasurementResult = base.Measure(availableSpace: availableSpace);

            return MeasurementResult.Value;
        }

        public override void Draw(
            Size availableSpace
        )
        {
            AvailableSpace = null;
            MeasurementResult = null;

            base.Draw(availableSpace: availableSpace);
        }

        private bool IsClose(
            float x
            , float y
        ) => Math.Abs(value: x - y) < Size.Epsilon;
    }
}