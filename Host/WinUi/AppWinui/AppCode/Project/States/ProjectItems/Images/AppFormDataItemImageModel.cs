using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppUsecases.Contracts.Entity;

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
    } = AppFormDataTypeEnum.Image;
    public ICollection<ImagesItem> ImagesItems
    {
        get; set;
    }
}
