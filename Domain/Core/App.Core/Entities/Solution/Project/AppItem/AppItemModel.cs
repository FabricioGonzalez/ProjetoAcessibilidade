﻿using System.Collections.Generic;

using AppUsecases.Project.Contracts.Entity;

namespace App.Core.Entities.Solution.Project.AppItem;
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
