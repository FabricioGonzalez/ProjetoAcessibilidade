﻿using AppUsecases.Entities.FileTemplate;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AppWinui.AppCode.Project.UITemplates.TemplateSelector;
public class ExplorerItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate FolderTemplate
    {
        get; set;
    }
    public DataTemplate FileTemplate
    {
        get; set;
    }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        return ((ExplorerItem)item).Type == ExplorerItemType.Folder ? FolderTemplate  : FileTemplate;
    }
}
