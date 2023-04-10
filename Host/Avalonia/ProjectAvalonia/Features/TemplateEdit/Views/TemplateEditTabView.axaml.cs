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
            name: nameof(SelectedItem)
        );


    public TemplateEditTabView()
    {
        InitializeComponent();

        this.WhenAnyValue(property1: v => v.SelectedItem)
            .WhereNotNull()
            .Subscribe(onNext: prop =>
            {
                (DataContext as TemplateEditTabViewModel).SelectedItem = prop;
            });
    }

    public FileItem? SelectedItem
    {
        get => GetValue(property: SelectedItemProperty);
        set => SetValue(property: SelectedItemProperty, value: value);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(obj: this);
}