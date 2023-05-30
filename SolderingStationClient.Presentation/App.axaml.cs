using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SolderingStationClient.Presentation.ViewModels.Interfaces;
using SolderingStationClient.Presentation.Views;
using Splat;

namespace SolderingStationClient.Presentation;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DataContext = Locator.Current.GetService<IMainWindowViewModel>() ??
                          throw new NullReferenceException(nameof(IMainWindowViewModel));
            
            //((IMainWindowViewModel)DataContext).Init();

            desktop.MainWindow = new MainWindow()
            {
                DataContext = DataContext
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}