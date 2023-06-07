using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SolderingStationClient.Presentation.Views;

public partial class ThermalProfileSelectorView : UserControl
{
    public ThermalProfileSelectorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}