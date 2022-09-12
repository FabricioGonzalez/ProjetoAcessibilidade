using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetoAcessibilidade.Controls.BasicControls;
public sealed partial class EditingLabel : UserControl
{
    public bool IsEditing
    {
        get => (bool)GetValue(IsEditingProperty);
        set => SetValue(IsEditingProperty, value);
    }

    // Using a DependencyProperty as the backing store for bool IsEditing.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsEditingProperty =
        DependencyProperty.Register(nameof(IsEditing), typeof(bool), typeof(EditingLabel), new PropertyMetadata(false));

    public string ItemText
    {
        get => (string)GetValue(ItemTextProperty);
        set => SetValue(ItemTextProperty, value);
    }

    // Using a DependencyProperty as the backing store for ItemText.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemTextProperty =
        DependencyProperty.Register(nameof(ItemText), typeof(string), typeof(EditingLabel), new PropertyMetadata(""));
     public string ItemEditingText
    {
        get => (string)GetValue(ItemEditingTextProperty);
        set => SetValue(ItemEditingTextProperty, value);
    }

    // Using a DependencyProperty as the backing store for ItemText.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemEditingTextProperty =
        DependencyProperty.Register(nameof(ItemEditingText), typeof(string), typeof(EditingLabel), new PropertyMetadata(""));

    public ICommand EnterCommand
    {
        get => (ICommand)GetValue(EnterCommandProperty);
        set => SetValue(EnterCommandProperty, value);
    }
    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EnterCommandProperty =
        DependencyProperty.RegisterAttached(nameof(EnterCommand), typeof(ICommand), typeof(EditingLabel), new PropertyMetadata(null,EnterCommandPropertyChanged));

    private static void EnterCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Debug.WriteLine(d);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(EditingLabel), new PropertyMetadata(null));

    public EditingLabel()
    {
        InitializeComponent();
    }

    private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            if (EnterCommand is not null)
            {
                EnterCommand.Execute(CommandParameter);
            }
        
        }
        if (e.Key == Windows.System.VirtualKey.Escape)
        {
            IsEditing = false;
        }
    }
}
