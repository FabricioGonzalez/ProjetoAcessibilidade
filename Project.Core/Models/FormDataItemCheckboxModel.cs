using Core.Contracts;
using Core.Enums;

using System.Collections.Generic;

namespace Core.Models;

public class FormDataItemCheckboxModel : IFormDataItemContract
{
    public string Topic
    {
        get; set;
    }
    public FormDataItemTypeEnum Type { get; set; } = FormDataItemTypeEnum.Checkbox;
    public IEnumerable<OptionModel> Options
    {
        get; set;
    }
}
