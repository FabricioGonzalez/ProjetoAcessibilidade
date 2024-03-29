﻿using ProjetoAcessibilidade.Core.Enuns;

namespace ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems;

public class AppFormDataEmptyModel : IAppFormDataItemContract
{
    public AppFormDataEmptyModel(
        string id = ""
        , string topic = ""
        , AppFormDataType type = AppFormDataType.Empty
    ) : base(id: id, topic: topic, type: AppFormDataType.Empty)
    {
    }
}