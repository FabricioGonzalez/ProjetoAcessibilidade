using System;
using System.Collections.Generic;

using AppUsecases.Project.Contracts.Entity;
using AppUsecases.Project.Enums;

namespace AppUsecases.Project.Entities.Project.AppFormDataItems.Images;
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
