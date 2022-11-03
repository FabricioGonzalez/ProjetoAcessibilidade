using AppUsecases.Contracts.Entity;
using AppUsecases.Project.Enums;

namespace AppWinui.AppCode.Project.States.ProjectItems.Images;
public class AppFormDataItemImageModel : IAppFormDataItemContract
{
    public string Topic
    {
        get;
        set;
    }
    public AppFormDataTypeEnum Type
    {
        get;
        set;
    } = AppFormDataTypeEnum.Images;
    public ICollection<ImagesItem> ImagesItems
    {
        get; set;
    }
}
