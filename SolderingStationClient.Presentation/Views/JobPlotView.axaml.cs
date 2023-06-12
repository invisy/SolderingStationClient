using Avalonia.Controls;
using Avalonia.Markup.Xaml;
namespace SolderingStationClient.Presentation.Views;

public class JobPlotView : UserControl
{
    public JobPlotView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}