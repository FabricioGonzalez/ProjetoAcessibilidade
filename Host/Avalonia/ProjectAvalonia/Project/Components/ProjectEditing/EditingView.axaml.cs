using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels.Project;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Project.Components.ProjectEditing;
public partial class EditingView : ReactiveUserControl<ItemEditingViewModel>
{
    public EditingView() 
    {
        ViewModel = Locator.Current.GetService<ItemEditingViewModel>();
      
        DataContext = ViewModel;

        this.WhenActivated(disposables =>
        {
           
        });

        AvaloniaXamlLoader.Load(this);
    }
}
