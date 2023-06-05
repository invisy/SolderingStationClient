using System;
using System.Linq;
using System.Threading.Tasks;
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
        _localizationService = Guard.Against.Null(localizationService);
    }

    public async Task Init()
    {
        var locales = await _localizationService.GetAvailableLocalizations();
        var selectedLocalizationCode = await _localizationService.GetCurrentLanguageCode();
        AvailableLanguages.AddRange(locales.Select(locale => new LanguageViewModel(locale)));
        SelectedLanguage = AvailableLanguages.First(lang => lang.Locale.CultureCode == selectedLocalizationCode);
    }

    public IAvaloniaList<LanguageViewModel> AvailableLanguages { get; } = new AvaloniaList<LanguageViewModel>();

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
        _localizationService.SaveSelectedLocalization(locale.Id).GetAwaiter().GetResult();
        
        var translations = Application.Current?.Resources.MergedDictionaries.OfType<ResourceInclude>()
            .FirstOrDefault(x => x.Source?.OriginalString.Contains("/Languages/") ?? false);

        if (translations == null) 
            return;
        
        Application.Current?.Resources.MergedDictionaries.Remove(translations);

        Application.Current?.Resources.MergedDictionaries.Add(
            new ResourceInclude
            {
                Source = new Uri(
                    $"avares://SolderingStationClient/Assets/Languages/{locale.CultureCode}.axaml")
            });
    }
}