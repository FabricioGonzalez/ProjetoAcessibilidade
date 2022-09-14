using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using SystemApplication.Services.UIOutputs;

namespace CustomControls.TemplateSelectors;
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
        var explorerItem = (ExplorerItem)item;
        return explorerItem.Type == ExplorerItem.ExplorerItemType.Folder ? FolderTemplate : FileTemplate;
    }
}

