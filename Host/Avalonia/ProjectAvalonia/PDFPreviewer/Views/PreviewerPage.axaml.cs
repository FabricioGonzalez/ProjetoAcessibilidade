using AppViewModels.PDFViewer;

using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace ProjectAvalonia.PDFPreviewer.Views;
public partial class PreviewerPage : ReactiveUserControl<PreviewerViewModel>
{
    public PreviewerPage()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
