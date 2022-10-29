using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;

using Microsoft.UI.Xaml;

namespace AppWinui.AppCode.AppUtils.Services;
public class InfoBarService
{
    public static InfoBar infobar;

    private InfoBar CreateInfoBar(
        string title,
        string message,
        InfoBarSeverity infoBarSeverity,
        bool hasAction = false,
        string buttonContent = "",
        ICommand Command = null,
        object CommandParamenter = null)
    {
       

        if (hasAction)
        {
            Button button = new Button();
            button.Command = Command;
            button.Content = buttonContent;
            button.CommandParameter = CommandParamenter;

            infobar.ActionButton = button;
        }
        else
        {
            infobar.ActionButton = null;
        }

        infobar.Title = title;
        infobar.Message = message;
        infobar.IsClosable = true;
        infobar.Severity = infoBarSeverity;

        infobar.VerticalAlignment = VerticalAlignment.Bottom;
        infobar.HorizontalAlignment = HorizontalAlignment.Right;

        infobar.Margin = new Thickness(
            0,
            0,
            20,
            20);

        infobar.CloseButtonCommand = new RelayCommand(() =>
        {
            infobar.IsOpen = false;
        });

        infobar.XamlRoot = App.MainWindow.Content.XamlRoot;

        return infobar;
    }

    public void SetMessageData(string title, string message, InfoBarSeverity infoBarSeverity)
    {
        var infobar = CreateInfoBar(title, message, infoBarSeverity);
        ShowInfo(infobar);
    }

    public void SetMessageData(
        string title,
        string message,
        InfoBarSeverity infoBarSeverity,
        bool hasAction,
        string buttonContent,
        ICommand Command,
        object CommandParamenter)
    {
        var infobar = CreateInfoBar(title, message, infoBarSeverity, hasAction, buttonContent, Command, CommandParamenter);
        ShowInfo(infobar);
    }

    void ShowInfo(InfoBar infoBar)
    {
        infoBar.IsOpen = true;
    }
}
