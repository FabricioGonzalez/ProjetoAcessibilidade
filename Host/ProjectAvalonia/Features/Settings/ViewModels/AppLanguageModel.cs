using ProjectAvalonia.Presentation.States.App;

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
        this LanguageState model
    ) => new(name: model.Name, code: model.Code);

    public static LanguageState ToLanguageModel(
        this AppLanguageModel model
    ) => new LanguageState(name: model.Name, code: model.Code);
}