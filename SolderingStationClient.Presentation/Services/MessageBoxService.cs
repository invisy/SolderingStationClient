using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using SolderingStationClient.Presentation.Models;

namespace SolderingStationClient.Presentation.Services;

public class MessageBoxService : IMessageBoxService
{
    private readonly IResourceProvider _resourceProvider;
    
    public MessageBoxService(IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
    }

    public async Task<ButtonResult> ShowMessageBoxWithLocalizedMessage(string key, MessageBoxType type, params object?[] arguments)
    {
        var line = _resourceProvider.GetResourceByName<string>(key);
        return await ShowMessageBox(string.Format(line, arguments), type);
    }

    public async Task<ButtonResult> ShowMessageBoxWithLocalizedMessage(string key, MessageBoxType type)
    {
        return await ShowMessageBox(_resourceProvider.GetResourceByName<string>(key), type);
    }

    public Task<ButtonResult> ShowMessageBox(string message, MessageBoxType type)
    {
        string title;
        
        switch (type)
        {
            case MessageBoxType.Info:
                title = _resourceProvider.GetResourceByName<string>("Localization.Info");
                return ShowMessageBox(title, message, Icon.Info);
            case MessageBoxType.Warning:
                title = _resourceProvider.GetResourceByName<string>("Localization.Warning");
                return ShowMessageBox(title, message, Icon.Warning);
            case MessageBoxType.Error:
                title = _resourceProvider.GetResourceByName<string>("Localization.Error");
                return ShowMessageBox(title, message, Icon.Error);
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private Task<ButtonResult> ShowMessageBox(string title, string message, Icon icon = Icon.None)
    {
        var lifetime = (IClassicDesktopStyleApplicationLifetime) Application.Current!.ApplicationLifetime!;
        
        var msBoxStandardWindow = MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.Ok,
                ContentTitle = title,
                ContentMessage = message,
                Icon = icon,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Topmost = true
            });
        
        return msBoxStandardWindow.ShowDialog(lifetime?.MainWindow);
    }
}