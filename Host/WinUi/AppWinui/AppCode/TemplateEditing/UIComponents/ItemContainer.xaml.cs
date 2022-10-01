using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;

using AppUsecases.Contracts.Entity;

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

namespace AppWinui.AppCode.TemplateEditing.UIComponents;
public sealed partial class ItemContainer : UserControl
{
    public IAppFormDataItemContract Item
    {
        get => (IAppFormDataItemContract)GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemProperty =
        DependencyProperty.Register(nameof(Item),
            typeof(IAppFormDataItemContract),
            typeof(ItemContainer),
            new PropertyMetadata(null));

    public List<string> ItemType = new()
    {
        "Texto",
        "Checkbox"
    };

    public ItemContainer()
    {
        InitializeComponent();
    }
}
