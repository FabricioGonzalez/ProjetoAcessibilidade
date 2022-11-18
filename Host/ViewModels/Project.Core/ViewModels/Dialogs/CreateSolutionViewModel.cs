using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.Project;

using Common;

using Project.Core.ViewModels.Extensions;

using ReactiveUI;

using Splat;

namespace Project.Core.ViewModels.Dialogs;
public class CreateSolutionViewModel : ViewModelBase
{
    private ICommandUsecase<ProjectSolutionModel, ProjectSolutionModel> solutionCreator;
    public CreateSolutionViewModel()
    {
        solutionCreator = Locator.Current.GetService<ICommandUsecase<ProjectSolutionModel, ProjectSolutionModel>>();

        CreateSolution = ReactiveCommand.CreateFromTask(async () =>
        {
            var result = await solutionCreator.executeAsync(new());

            return result;
        });

    }


    public ReactiveCommand<Unit, Resource<ProjectSolutionModel>> CreateSolution
    {
        get; set;
    }
}
