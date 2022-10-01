
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWinui.AppCode.AppUtils.UIComponents;
public sealed partial class ConditionalTextEditingBlock : UserControl
{
    public bool IsEditing
    {
        get => (bool)GetValue(IsEditingProperty);
        set => SetValue(IsEditingProperty, value);
    }

    public static readonly DependencyProperty IsEditingProperty =
        DependencyProperty.Register("IsEditing",
            typeof(bool),
            typeof(ConditionalTextEditingBlock),
            new PropertyMetadata(false));
    public string EditableText
    {
        get => (string)GetValue(EditableTextProperty);
        set => SetValue(EditableTextProperty, value);
    }

    public static readonly DependencyProperty EditableTextProperty =
        DependencyProperty.Register(nameof(EditableText),
            typeof(string),
            typeof(ConditionalTextEditingBlock),
            new PropertyMetadata(""));

    public ConditionalTextEditingBlock()
    {
        this.InitializeComponent();
    }

    private void TextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (sender is TextBox txt)
        {
            if (e.Key is Windows.System.VirtualKey.Enter)
            {
                IsEditing = false;

                EditableText = txt.Text;

                txt.Text = "";
            }
            if (e.Key is Windows.System.VirtualKey.Escape)
            {
                IsEditing = false;
                txt.Text = "";
            }
        }

    }
}
