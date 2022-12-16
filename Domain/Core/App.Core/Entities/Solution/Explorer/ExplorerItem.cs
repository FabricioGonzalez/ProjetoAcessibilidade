﻿using App.Core.Common;

namespace App.Core.Entities.Solution.Explorer;
public class ExplorerItem : BaseAuditableEntity
{
    public string Name
    {
        get; set;
    }
    public string Path
    {
        get; set;
    }

    public string ReferencedItem
    {
        get; set;
    }
}
