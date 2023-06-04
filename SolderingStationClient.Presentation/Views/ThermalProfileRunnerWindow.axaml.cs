using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SolderingStationClient.Presentation.Views;

public partial class ThermalProfileRunnerWindow : Window
{
    public ThermalProfileRunnerWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}