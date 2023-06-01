using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SolderingStation.DAL.Implementation;
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
            // Initialize database
            var solderingStationContext = Locator.Current.GetService<SolderingStationDbContext>() ?? 
                                          throw new NullReferenceException(nameof(SolderingStationDbContext));
            solderingStationContext.Seed();
            
            //Load viewmodel and open main window
            DataContext = Locator.Current.GetService<IMainWindowViewModel>() ??
                          throw new NullReferenceException(nameof(IMainWindowViewModel));

            desktop.MainWindow = new MainWindow()
            {
                DataContext = DataContext
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}