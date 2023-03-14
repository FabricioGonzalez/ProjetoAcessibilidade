using Project.Domain.App.Models;

namespace ProjectAvalonia.Features.Settings.ViewModels;
public class AppLanguageModel
{
    public string Name
    {
        get;
    }

    public string NativeName
    {
        get;
    }

    public string Code
    {
        get;
    }

    public AppLanguageModel(string name, string nativeName, string code)
    {
        Name = name;
        NativeName = nativeName;
        Code = code;
    }
}

public static class Extensions
{
    public static AppLanguageModel ToAppLanguageModel(this LanguageModel model)
    {
        return new AppLanguageModel(name: model.Name, nativeName: model.NativeName, code: model.Code);
    }
    public static LanguageModel ToLanguageModel(this AppLanguageModel model)
    {
        return new LanguageModel(name: model.Name, nativeName: model.NativeName, code: model.Code);
    }
}
