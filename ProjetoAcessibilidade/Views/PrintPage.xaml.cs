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

using ProjetoAcessibilidade.ViewModels;

using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetoAcessibilidade.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PrintPage : Page
{
    public PrintViewModel ViewModel
    {
        get;private set;
    }
    public PrintPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<PrintViewModel>();
    }

    //private void RichEditBox_Loaded(object sender, RoutedEventArgs e)
    //{
    //    if (sender is RichEditBox editBox)
    //    {
    //        editBox.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, ViewModel.File);
    //    }
    //}
}
