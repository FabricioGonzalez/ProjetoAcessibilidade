using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppUsecases.Contracts.Entity;
using AppUsecases.Project.Enums;

using ReactiveUI;

namespace AppWinui.AppCode.Project.States.ProjectItems.Observations;
public class AppFormDataItemObservationModel : ReactiveObject,IAppFormDataItemContract
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

    private string _observation = "";
    public string Observation
    {
        get => _observation; 
        set => this.RaiseAndSetIfChanged(
            ref _observation,
            value,
            nameof(Observation));
    }
}
