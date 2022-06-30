using Core.Enums;

namespace Core.Contracts;

public interface IFormDataItemContract
{
    public string Topic { get; set; }
    public FormDataItemTypeEnum Type { get; set; }
}
