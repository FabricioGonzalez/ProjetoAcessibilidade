using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Section
        : ContainerElement
            , IStateResettable
    {
        public string LocationName
        {
            get;
            set;
        }

        private bool IsRendered
        {
            get;
            set;
        }

        public void ResetState() => IsRendered = false;

        public override void Draw(
            Size availableSpace
        )
        {
            if (!IsRendered)
            {
                Canvas.DrawSection(sectionName: LocationName);
                IsRendered = true;
            }

            PageContext.SetSectionPage(name: LocationName);
            base.Draw(availableSpace: availableSpace);
        }
    }
}