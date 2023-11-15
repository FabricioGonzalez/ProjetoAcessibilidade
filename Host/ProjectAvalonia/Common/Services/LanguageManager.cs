using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Interfaces;
using ProjectAvalonia.Common.Models;
using ProjectAvalonia.Presentation.States.App;

namespace ProjectAvalonia.Common.Services;

public class LanguageManager : ILanguageManager
{
    private readonly Lazy<Dictionary<string, LanguageState>> _availableLanguages;
    private readonly LanguagesConfiguration _configuration;

    public LanguageManager(
        LanguagesConfiguration configuration
    )
    {
        _configuration = configuration;
        _availableLanguages = new Lazy<Dictionary<string, LanguageState>>(GetAvailableLanguages);

        DefaultLanguage = CreateLanguageModel(CultureInfo.GetCultureInfo("en"));
    }

    public LanguageState DefaultLanguage
    {
        get;
    }

    public LanguageState CurrentLanguage => CreateLanguageModel(CultureInfo.InstalledUICulture);

    public IEnumerable<LanguageState> AllLanguages => _availableLanguages.Value.Values;

    public void SetLanguage(
        string languageCode
    )
    {
        if (!string.IsNullOrEmpty(languageCode))
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(languageCode);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(languageCode);
        }
    }

    public void SetLanguage(
        LanguageState languageModel
    ) => SetLanguage(languageModel.Code);

    private Dictionary<string, LanguageState> GetAvailableLanguages() =>
        _configuration
            .AvailableLocales
            .Select(locale => CreateLanguageModel(new CultureInfo(locale)))
            .ToDictionary(keySelector: lm => lm.Code, elementSelector: lm => lm);

    private LanguageState CreateLanguageModel(
        CultureInfo? cultureInfo
    ) =>
        cultureInfo is null
            ? DefaultLanguage
            : new LanguageState(code: cultureInfo.TwoLetterISOLanguageName, name: cultureInfo.NativeName.ToTitleCase());
}