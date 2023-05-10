using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Models;
using ProjetoAcessibilidade.Domain.App.Contracts;
using ProjetoAcessibilidade.Domain.App.Models;

namespace ProjectAvalonia.Common.Services;

public class LanguageManager : ILanguageManager
{
    private readonly Lazy<Dictionary<string, LanguageModel>> _availableLanguages;
    private readonly LanguagesConfiguration _configuration;

    public LanguageManager(
        LanguagesConfiguration configuration
    )
    {
        _configuration = configuration;
        _availableLanguages = new Lazy<Dictionary<string, LanguageModel>>(GetAvailableLanguages);

        DefaultLanguage = CreateLanguageModel(CultureInfo.GetCultureInfo("en"));
    }

    public LanguageModel DefaultLanguage
    {
        get;
    }

    public LanguageModel CurrentLanguage => CreateLanguageModel(Thread.CurrentThread.CurrentUICulture);

    public IEnumerable<LanguageModel> AllLanguages => _availableLanguages.Value.Values;

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
        LanguageModel languageModel
    ) => SetLanguage(languageModel.Code);

    private Dictionary<string, LanguageModel> GetAvailableLanguages() =>
        _configuration
            .AvailableLocales
            .Select(locale => CreateLanguageModel(new CultureInfo(locale)))
            .ToDictionary(lm => lm.Code, lm => lm);

    private LanguageModel CreateLanguageModel(
        CultureInfo cultureInfo
    ) =>
        cultureInfo is null
            ? DefaultLanguage
            : new LanguageModel(cultureInfo.EnglishName, cultureInfo.NativeName.ToTitleCase(),
                cultureInfo.TwoLetterISOLanguageName);
}