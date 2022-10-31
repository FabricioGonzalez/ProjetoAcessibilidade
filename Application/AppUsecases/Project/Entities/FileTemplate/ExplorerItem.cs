using System;
using System.Collections.Generic;

namespace AppUsecases.Entities.FileTemplate;
public class ExplorerItem
{
    public string Name
    {
        get; set;
    }
    public string Path
    {
        get; set;
    }
    public ExplorerItemType Type
    {
        get; set;
    }
    public IList<ExplorerItem> Children
    {
        get;set;
    }
}
