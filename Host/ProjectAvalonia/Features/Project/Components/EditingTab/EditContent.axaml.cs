using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Project.Components.EditingTab;

<<<<<<<< HEAD:Host/Avalonia/ProjectAvalonia/Features/Project/Components/EditingBody/EditingForm.axaml.cs
public partial class EditingForm : UserControl
{
    public EditingForm()
========
public partial class EditContent : UserControl
{
    public EditContent()
>>>>>>>> d1a922c2b15b1ed4309eb6c79bac6e8d9315b26f:Host/ProjectAvalonia/Features/Project/Components/EditingTab/EditContent.axaml.cs
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}