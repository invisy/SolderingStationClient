using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SolderingStationClient.Presentation.Views;

public class DeviceView : UserControl
{
    public DeviceView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}