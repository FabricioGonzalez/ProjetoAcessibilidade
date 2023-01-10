using System;
using System.Reactive.Disposables;

using AppViewModels.TemplateEditing;
using AppViewModels.TemplateEditing.Models;

using Avalonia;
using Avalonia.ReactiveUI;

using DynamicData.Binding;

using ProjectAvalonia.Project.Components.ProjectEditing.EditingItems;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.TemplateEditing.Components.Page;
public partial class TemplateEditingPage : ReactiveUserControl<TemplateEditingPageViewModel>
{

    public static readonly AttachedProperty<ItemTemplateModel> ItemProperty =
      AvaloniaProperty.RegisterAttached<EditingItem, ReactiveUserControl<TemplateEditingPageViewModel>, ItemTemplateModel>(nameof(Item));

    public ItemTemplateModel? Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }
    public TemplateEditingPage()
    {
        ViewModel = Locator.Current.GetService<TemplateEditingPageViewModel>();

        DataContext = ViewModel;

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            ViewModel.Activator.Activate();

            this.WhenPropertyChanged(v => v.Item)
            .Subscribe(async prop =>
            {
                if (prop.Value is not null)
                {
                    await ViewModel.SetEditingItem(prop.Value.Path);
                }
            });



        });

        InitializeComponent();
    }
}
