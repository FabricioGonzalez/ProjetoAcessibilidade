using Core.Contracts;
using Core.Enums;

using System.Collections.Generic;

namespace Core.Models;

internal class FormDataItemCheckboxModel : IFormDataItemContract
{
    public string Topic { get; set; }
    public FormDataItemTypeEnum Type { get; set; }
    public IEnumerable<OptionModel> Options { get; set; }
}
