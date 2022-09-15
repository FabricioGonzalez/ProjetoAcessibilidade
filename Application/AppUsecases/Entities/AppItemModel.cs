using System.Collections.Generic;

using AppUsecases.Contracts.Entity;

namespace AppUsecases.Entities;
public class AppItemModel
{
    
        public string ItemName
        {
            get; set;
        }
        public IList<IAppFormDataItemContract> FormData
        {
            get; set;
        }
        public IList<AppLawModel> LawList
        {
            get; set;
        }
    
}
