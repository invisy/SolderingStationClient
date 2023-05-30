using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SolderingStationClient.Presentation.Views;

public class PidTemperatureControllerView : UserControl
{
    public PidTemperatureControllerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-Us");
    }
}