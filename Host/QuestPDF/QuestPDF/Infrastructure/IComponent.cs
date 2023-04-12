using QuestPDF.Elements;

namespace QuestPDF.Infrastructure
{
    internal interface ISlot
    {
    }

    internal class Slot
        : Container
            , ISlot
    {
    }

    public interface IComponent
    {
        void Compose(
            IContainer container
        );
    }
}