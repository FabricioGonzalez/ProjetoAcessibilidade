using System.Collections.Generic;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing.Proxy
{
    public class DebugStackItem
    {
        public IElement Element
        {
            get;
            set;
        }

        public Size AvailableSpace
        {
            get;
            set;
        }

        public SpacePlan SpacePlan
        {
            get;
            set;
        }

        public ICollection<DebugStackItem> Stack
        {
            get;
            set;
        } = new List<DebugStackItem>();
    }
}