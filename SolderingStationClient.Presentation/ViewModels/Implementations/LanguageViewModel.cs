using SolderingStationClient.Models;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class LanguageViewModel : ViewModelBase
{
    public LanguageViewModel(Locale locale)
    {
        Locale = locale;
        Name = $"{locale.EnglishName} ({locale.NativeName})";
    }

    public Locale Locale { get; }
    public string Name { get; }
}