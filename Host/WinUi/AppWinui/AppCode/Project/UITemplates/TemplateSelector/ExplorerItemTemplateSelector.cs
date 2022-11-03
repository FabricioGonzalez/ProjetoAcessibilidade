
using AppUsecases.Project.Entities.FileTemplate;
using AppUsecases.Project.Enums;

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
        var explorer = (ExplorerItem)item;
        return typeof(FolderItem) == explorer.GetType() ? FolderTemplate  : FileTemplate;
    }
}
