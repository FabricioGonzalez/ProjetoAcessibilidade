using System;
using System.Collections.Generic;

using AppUsecases.Contracts.Entity;

namespace AppUsecases.Entities.AppFormDataItems.Images;
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
