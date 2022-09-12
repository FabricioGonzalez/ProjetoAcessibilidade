using Core.Contracts;
using Core.Enums;

namespace Core.Models;

public class FormDataItemCheckboxModel : IFormDataItemContract
{
    public string Topic
    {
        get; set;
    }
    public FormDataItemTypeEnum Type { get; set; } = FormDataItemTypeEnum.Checkbox;
    public List<FormDataItemCheckboxChildModel> Children
    {
        get; set;
    }

}
