using System.Collections.Generic;
using System.Linq;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace ProjectAvalonia.Features.SearchBar.Settings;

public class SettingSelector : IDataTemplate
{
    public List<IDataTemplate> DataTemplates { get; set; } = new();

    public IControl Build(object param)
    {
        var prop = param.GetType().GetProperty("Value");
        var template = DataTemplates.FirstOrDefault(predicate: d =>
        {
            var value = prop?.GetValue(obj: param);

            if (value is null)
            {
                return false;
            }

            return d.Match(data: value);
        });

        if (template is not null)
        {
            return template.Build(param);
        }

        return new TextBlock { Text = "Not found" };
    }

    public bool Match(object data)
    {
        return data.GetType().Name.Contains(value: "Setting");
    }
}
