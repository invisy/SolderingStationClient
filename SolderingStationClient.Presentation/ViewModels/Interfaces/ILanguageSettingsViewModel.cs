using System;
using System.Threading.Tasks;
using Avalonia.Collections;
using SolderingStationClient.Presentation.ViewModels.Implementations;

namespace SolderingStationClient.Presentation.ViewModels.Interfaces;

public interface ILanguageSettingsViewModel : IViewModelBase
{
    public Task Init();
    public IAvaloniaList<LanguageViewModel> AvailableLanguages { get; }
    public LanguageViewModel SelectedLanguage { get; set; }
}