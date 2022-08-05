using System.Collections.ObjectModel;

using Core.Models;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CustomControls;
public sealed partial class ProjectItemTemplate : UserControl
{
    #region DependencyProperties

    public ObservableCollection<FormDataItemModel> Items
    {
        get => (ObservableCollection<FormDataItemModel>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register("Items", typeof(ObservableCollection<FormDataItemModel>),
            typeof(ProjectItemTemplate), new PropertyMetadata(new ObservableCollection<FormDataItemModel>()));
    #endregion

    #region Constructors
    public ProjectItemTemplate()
    {
        InitializeComponent();
    } 
    #endregion
}
