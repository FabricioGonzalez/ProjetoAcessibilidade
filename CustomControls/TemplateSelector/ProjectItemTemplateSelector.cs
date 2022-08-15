using Core.Contracts;
using Core.Enums;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CustomControls.TemplateSelectors;
public class ProjectItemTemplateSelector : DataTemplateSelector
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
        var projectItem = (IFormDataItemContract)item;

        if (projectItem.Type == FormDataItemTypeEnum.Observation)
        {
            return ObservationTemplate;
        }

        if (projectItem.Type == FormDataItemTypeEnum.Checkbox)
        {
            return CheckBoxTemplate;
        }

        if (projectItem.Type == FormDataItemTypeEnum.Text)
        {
            return TextBoxTemplate;
        }
        return TextBoxTemplate;
    }
}
