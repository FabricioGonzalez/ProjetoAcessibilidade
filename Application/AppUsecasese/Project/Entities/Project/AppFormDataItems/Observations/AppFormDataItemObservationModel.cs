using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppUsecases.Contracts.Entity;
using AppUsecases.Project.Enums;

namespace AppUsecases.Entities.AppFormDataItems.Observations;
public class AppFormDataItemObservationModel : IAppFormDataItemContract
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
    } = AppFormDataTypeEnum.Observation;
    public string Observation
    {
        get; set;
    }
}
