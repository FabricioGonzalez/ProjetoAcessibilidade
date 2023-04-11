﻿using Core.Enuns;

namespace ProjectAvalonia.Features.Project.States.FormItemState;

public partial class ObservationItemState : FormItemStateBase
{
    [AutoNotify]
    private string _observation = "";

    [AutoNotify]
    private string _topic = "";

    public ObservationItemState(
        string topic
        , string observation
        , AppFormDataType type = AppFormDataType.Observação
        , string id = ""
    )
        : base(type: type, id: id)
    {
        Observation = observation;
        Topic = topic;
    }
}