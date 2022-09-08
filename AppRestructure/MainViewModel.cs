﻿using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

using AppRestructure.Project.ViewModels;

using ReactiveUI;

using Splat;

namespace AppRestructure;

public class MainViewModel : ReactiveObject, IScreen
{
    // The Router associated with this Screen.
    // Required by the IScreen interface.
    public RoutingState Router
    {
        get;
    }

    // The command that navigates a user to first view model.
    public ReactiveCommand<Unit, IRoutableViewModel> GoNext
    {
        get;
    }

    // The command that navigates a user back.
    public ReactiveCommand<Unit, Unit> GoBack
    {
        get;
    }

    public MainViewModel()
    {
        // Initialize the Router.
        Router = new RoutingState();

        // Router uses Splat.Locator to resolve views for
        // view models, so we need to register our views
        // using Locator.CurrentMutable.Register* methods.
        //
        // Instead of registering views manually, you 
        // can use custom IViewLocator implementation,
        // see "View Location" section for details.
        //
        Locator.CurrentMutable.Register(() => new ProjectViewModel(this), typeof(IViewFor<ProjectViewModel>));

        // Manage the routing state. Use the Router.Navigate.Execute
        // command to navigate to different view models. 
        //
        // Note, that the Navigate.Execute method accepts an instance 
        // of a view model, this allows you to pass parameters to 
        // your view models, or to reuse existing view models.
        //
        GoNext = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ProjectViewModel(this)));

        // You can also ask the router to go back. One option is to 
        // execute the default Router.NavigateBack command. Another
        // option is to define your own command with custom
        // canExecute condition as such:
        //var canGoBack = this
        //    .WhenAnyValue(x => x.Router.NavigationStack.Count)
        //    .Select(count => count > 0);
        //GoBack = ReactiveCommand.CreateFromObservable(
        //    () => Router.NavigateBack.Execute(Unit.Default),
        //    canGoBack);
    }
}
