using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using ProjectAvalonia.Common.Models.FileItems;
using ProjectAvalonia.Features.TemplateEdit.ViewModels;

using ReactiveUI;

namespace ProjectAvalonia.Features.TemplateEdit.Views;
public partial class TemplateEditTabView : UserControl
{
    public static readonly AttachedProperty<FileItem> SelectedItemProperty =
      AvaloniaProperty.RegisterAttached<TemplateEditTabView, TemplateEditTabView, FileItem>(
          nameof(SelectedItem)
          );
    public FileItem? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }


    public TemplateEditTabView()
    {
        InitializeComponent();

        this.WhenAnyValue(v => v.SelectedItem)
            .WhereNotNull()
            .Subscribe((prop) =>
            {
                (DataContext as TemplateEditTabViewModel).SelectedItem = prop;
            });
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
