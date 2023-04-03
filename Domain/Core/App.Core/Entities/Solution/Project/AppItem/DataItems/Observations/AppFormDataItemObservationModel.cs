﻿using Core.Enuns;

namespace Core.Entities.Solution.Project.AppItem.DataItems.Observations;
public class AppFormDataItemObservationModel : IAppFormDataItemContract
{
    public AppFormDataItemObservationModel(
        string observation,
        string id,
        string topic,
        AppFormDataType type = AppFormDataType.Observação)
        : base(id, topic, type)
    {
        Observation = observation;
    }
    public string Observation
    {
        get; set;
    }
}
