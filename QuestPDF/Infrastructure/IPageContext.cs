using System.Collections.Generic;

namespace QuestPDF.Infrastructure
{
    public class DocumentLocation
    {
        public string Name { get; set; }
        public int PageStart { get; set; }
        public int PageEnd { get; set; }
        public int Length => PageEnd - PageStart + 1;
    }
    
    public interface IPageContext
    {
        int CurrentPage { get; }
        void SetSectionPage(string name);
        DocumentLocation? GetLocation(string name);
    }
}