using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ProjectAvalonia.Common.ViewModels;

namespace ProjectAvalonia;

[StaticViewLocator]
public partial class ViewLocator : IDataTemplate
{
    public Control Build(
        object data
    )
    {
        var type = data.GetType();
        if (s_views.TryGetValue(key: type, value: out var func))
        {
            return func.Invoke();
        }

        throw new Exception(message: $"Unable to create view for type: {type}");
    }

    public bool Match(
        object data
    ) => data is ViewModelBase;
}