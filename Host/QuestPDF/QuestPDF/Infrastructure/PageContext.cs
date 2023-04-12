using System.Collections.Generic;
using System.Linq;

namespace QuestPDF.Infrastructure
{
    public class PageContext : IPageContext
    {
        public const string DocumentLocation = "document";

        private List<DocumentLocation> Locations
        {
            get;
        } = new();

        public int CurrentPage
        {
            get;
            private set;
        }

        public void SetSectionPage(
            string name
        )
        {
            var location = GetLocation(name: name);

            if (location == null)
            {
                location = new DocumentLocation
                {
                    Name = name, PageStart = CurrentPage, PageEnd = CurrentPage
                };

                Locations.Add(item: location);
            }

            if (location.PageEnd < CurrentPage)
            {
                location.PageEnd = CurrentPage;
            }
        }

        public DocumentLocation? GetLocation(
            string name
        ) => Locations.FirstOrDefault(predicate: x => x.Name == name);

        public void SetPageNumber(
            int number
        )
        {
            CurrentPage = number;
            SetSectionPage(name: DocumentLocation);
        }
    }
}