using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SolderingStationClient.Presentation.Views;

public class DevicesListView : UserControl
{
    public DevicesListView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}