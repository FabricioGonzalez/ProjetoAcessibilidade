using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppRestructure.Project.ViewModels;
using AppRestructure.Project.Views;

using ReactiveUI;

using Splat;

namespace AppRestructure;
//public class ViewLocator : IViewLocator
//{
//    public IViewFor ResolveView<T>(T viewModel, string contract = null) where T : class
//    {
//        if (viewModel is ProjectViewModel)
//            return new ProjectPage { ViewModel = viewModel };
//        throw new Exception($"Could not find the view for view model {typeof(T).Name}.");
//    }
//}

////// Register the SimpleViewLocator.
////Locator.
////    CurrentMutable.RegisterLazySingleton(() => new ViewLocator(), typeof(IViewLocator));
