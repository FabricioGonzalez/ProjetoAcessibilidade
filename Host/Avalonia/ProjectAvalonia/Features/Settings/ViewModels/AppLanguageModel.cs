using Domain.Internals;

namespace ProjectAvalonia.Features.Settings.ViewModels;

public class AppLanguageModel
{
    public AppLanguageModel(
        string name
        , string code
    )
    {
        Name = name;
        Code = code;
    }

    public string Name
    {
        get;
    }


    public string Code
    {
        get;
    }
}

public static class Extensions
{
    public static AppLanguageModel ToAppLanguageModel(
        this ILanguage model
    ) => new(name: model.Name, code: model.Code);

    public static ILanguage ToLanguageModel(
        this AppLanguageModel model
    ) => new Language(name: model.Name, code: model.Code);
}