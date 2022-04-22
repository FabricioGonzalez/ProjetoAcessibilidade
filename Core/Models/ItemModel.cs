using System.Collections.Generic;

namespace Core
{
    public class ItemModel
    {
        public string ItemName { get; set; }
        public IEnumerable<FormDataModel> FormData { get; set; }
        public IEnumerable<LawModel> LawList { get; set; }
    }
}
