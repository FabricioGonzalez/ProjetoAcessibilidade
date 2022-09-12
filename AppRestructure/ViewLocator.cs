using System;

using AppRestructure.Project.ViewModels;
using AppRestructure.Project.Views;

using ReactiveUI;

using Splat;

namespace AppRestructure;
public class ViewLocator : IViewLocator
{
    IViewFor IViewLocator.ResolveView<T>(T viewModel, string? contract)
    {
        if (viewModel is ProjectViewModel vm)
            return new ProjectPage { ViewModel = vm };
        throw new Exception($"Could not find the view for view model {typeof(T).Name}.");
    }
}
