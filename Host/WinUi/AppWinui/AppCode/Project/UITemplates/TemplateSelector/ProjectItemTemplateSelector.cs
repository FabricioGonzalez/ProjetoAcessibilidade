using AppUsecases.Contracts.Entity;
using AppUsecases.Project.Enums;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AppWinui.AppCode.Project.UITemplates.TemplateSelector;
internal class ProjectItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate CheckBoxTemplate
    {
        get; set;
    }
    public DataTemplate TextBoxTemplate
    {
        get; set;
    }
    public DataTemplate ObservationTemplate
    {
        get; set;
    }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        var projectItem = (IAppFormDataItemContract)item;

        if (projectItem.Type == AppFormDataTypeEnum.Observation)
        {
            return ObservationTemplate;
        }

        if (projectItem.Type == AppFormDataTypeEnum.Checkbox)
        {
            return CheckBoxTemplate;
        }

        if (projectItem.Type == AppFormDataTypeEnum.Text)
        {
            return TextBoxTemplate;
        }
        return TextBoxTemplate;
    }
}
