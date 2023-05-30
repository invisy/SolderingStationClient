using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Markup.Xaml.MarkupExtensions;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class LanguageSettingsViewModel : ViewModelBase, ILanguageSettingsViewModel
{
    private readonly ILocalizationService _localizationService;

    private LanguageViewModel _selectedLanguage = default!;

    public LanguageSettingsViewModel(ILocalizationService localizationService)
    {
        _localizationService = Guard.Against.Null(localizationService);;

        var viewModels = localizationService.GetLanguagesByCodesList(new List<string>(new[] { "en", "uk" }))
            .Select(x => new LanguageViewModel(x));
        AvailableLanguages = new AvaloniaList<LanguageViewModel>(viewModels);

        //TODO initialize Selected language with database value
        SelectedLanguage = AvailableLanguages.First(lang =>
            lang.Locale.CultureCode == _localizationService.GetCurrentLanguageCode());
    }

    public IAvaloniaList<LanguageViewModel> AvailableLanguages { get; }

    public LanguageViewModel SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            UpdateLocalization(value.Locale);
            this.RaiseAndSetIfChanged(ref _selectedLanguage, value);
        }
    }

    private void UpdateLocalization(Locale locale)
    {
        //TODO save new value

        var translations = Application.Current?.Resources.MergedDictionaries.OfType<ResourceInclude>()
            .FirstOrDefault(x => x.Source?.OriginalString?.Contains("/Languages/") ?? false);

        if (translations != null)
            Application.Current?.Resources.MergedDictionaries.Remove(translations);

        Application.Current?.Resources.MergedDictionaries.Add(
            new ResourceInclude
            {
                Source = new Uri(
                    $"avares://SolderingStationClient/Assets/Languages/{locale.CultureCode}.axaml")
            });
    }
}