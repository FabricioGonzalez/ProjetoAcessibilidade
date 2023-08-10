using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Application.Internals.Contracts;
using Domain.Internals;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Models;

namespace ProjectAvalonia.Common.Services;

public class LanguageManager : ILanguageManager
{
    private readonly Lazy<Dictionary<string, ILanguage>> _availableLanguages;
    private readonly LanguagesConfiguration _configuration;

    public LanguageManager(
        LanguagesConfiguration configuration
    )
    {
        _configuration = configuration;
        _availableLanguages = new Lazy<Dictionary<string, ILanguage>>(GetAvailableLanguages);

        DefaultLanguage = CreateLanguageModel(CultureInfo.GetCultureInfo("en"));
    }

    public ILanguage DefaultLanguage
    {
        get;
    }

    public ILanguage CurrentLanguage => CreateLanguageModel(CultureInfo.InstalledUICulture);

    public IEnumerable<ILanguage> AllLanguages => _availableLanguages.Value.Values;

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
        ILanguage languageModel
    ) => SetLanguage(languageModel.Code);

    private Dictionary<string, ILanguage> GetAvailableLanguages() =>
        _configuration
            .AvailableLocales
            .Select(locale => CreateLanguageModel(new CultureInfo(locale)))
            .ToDictionary(keySelector: lm => lm.Code, elementSelector: lm => lm);

    private ILanguage CreateLanguageModel(
        CultureInfo? cultureInfo
    ) =>
        cultureInfo is null
            ? DefaultLanguage
            : new Language(code: cultureInfo.TwoLetterISOLanguageName, name: cultureInfo.NativeName.ToTitleCase());
}