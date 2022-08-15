using Core.Contracts;
using Core.Enums;

namespace SystemApplication.Services.UIOutputs;

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
