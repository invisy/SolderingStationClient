using System.Threading.Tasks;
using MessageBox.Avalonia.Enums;
using SolderingStationClient.Presentation.Models;

namespace SolderingStationClient.Presentation.Services;

public interface IMessageBoxService
{
    Task<ButtonResult> ShowMessageBoxWithLocalizedMessage(string key, MessageBoxType type, params object?[] arguments);
    Task<ButtonResult> ShowMessageBoxWithLocalizedMessage(string key, MessageBoxType type);
    Task<ButtonResult> ShowMessageBox(string message, MessageBoxType type);
}