using System.Collections.Generic;
using ProjectAvalonia.Presentation.States.App;

namespace ProjectAvalonia.Common.Interfaces;

public interface ILanguageManager
{
    public LanguageState DefaultLanguage
    {
        get;
    }

    public LanguageState CurrentLanguage
    {
        get;
    }

    public IEnumerable<LanguageState> AllLanguages
    {
        get;
    }

    public void SetLanguage(
        string languageCode
    );

    public void SetLanguage(
        LanguageState languageModel
    );
}