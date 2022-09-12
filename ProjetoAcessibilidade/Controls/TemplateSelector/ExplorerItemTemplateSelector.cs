using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using SystemApplication.Services.UIOutputs;
using ProjetoAcessibilidade.Modules.Project.ViewModels;

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

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        var explorerItem = (ExplorerItem)item;

        (container as FrameworkElement).DataContext = new ExplorerItemViewModel(explorerItem);

        var result =  explorerItem.Type == ExplorerItem.ExplorerItemType.Folder ? FolderTemplate : FileTemplate;
        
        //result.

        return result;
    }
}

