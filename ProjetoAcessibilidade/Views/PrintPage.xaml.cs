using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using ProjetoAcessibilidade.Services;
using ProjetoAcessibilidade.ViewModels;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetoAcessibilidade.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PrintPage : Page
{
    private PrintHelper printHelper;
    public PrintViewModel ViewModel
    {
        get; private set;
    }
    private async void OnPrintButtonClick(object sender, RoutedEventArgs e)
    {
        await printHelper.ShowPrintUIAsync();
    }


    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        if (printHelper != null)
        {
            printHelper.UnregisterForPrinting();
        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if (PrintManager.IsSupported())
        {
            // Tell the user how to print
            //MainPage.Current.NotifyUser("Print contract registered with customization, use the Print button to print.", NotifyType.StatusMessage);
        }
        else
        {
            // Remove the print button
            InvokePrintingButton.Visibility = Visibility.Collapsed;

            // Inform user that Printing is not supported
            //MainPage.Current.NotifyUser("Printing is not supported.", NotifyType.ErrorMessage);

            // Printing-related event handlers will never be called if printing
            // is not supported, but it's okay to register for them anyway.
        }

        // Initalize common helper class and register for printing
        printHelper = new PrintHelper(this);
        printHelper.RegisterForPrinting();

        // Initialize print content for this scenario
        printHelper.PreparePrintContent(new PageToPrint());
    }
}
