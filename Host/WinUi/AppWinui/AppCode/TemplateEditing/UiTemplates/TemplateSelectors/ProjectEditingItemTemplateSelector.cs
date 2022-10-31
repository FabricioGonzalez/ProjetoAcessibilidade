using AppUsecases.Contracts.Entity;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AppWinui.AppCode.TemplateEditing.UiTemplates.TemplateSelectors;
public class ProjectEditingItemTemplateSelector : DataTemplateSelector
{

    public DataTemplate CheckBoxTemplate
    {
        get; set;
    }
    public DataTemplate TextBoxTemplate
    {
        get; set;
    }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        var projectItem = (IAppFormDataItemContract)item;

        if (projectItem.Type == AppFormDataTypeEnum.Checkbox)
        {
            return CheckBoxTemplate;
        }
        if (projectItem.Type == AppFormDataTypeEnum.Text)
            return TextBoxTemplate;

        return null;
    }
}
