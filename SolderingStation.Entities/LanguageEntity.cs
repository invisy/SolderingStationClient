namespace SolderingStation.Entities;

public class LanguageEntity : BaseEntity<int>
{
    public string EnglishName { get; }
    public string NativeName { get; }
    public string Code { get; }
}