﻿using App.Core.Entities.Solution;

namespace Project.Application.Solution.Contracts;
public interface ISolutionRepository
{
    public Task<ProjectSolutionModel> ReadSolution(string solutionPath);
    public Task CreateSolution(string solutionPath);
    public Task SaveSolution(string solutionPath, ProjectSolutionModel dataToWrite);
}