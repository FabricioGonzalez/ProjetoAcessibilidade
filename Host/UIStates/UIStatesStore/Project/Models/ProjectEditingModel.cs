﻿using AppUsecases.Project.Entities.Project;

namespace UIStatesStore.Project.Models;
public class ProjectEditingModel
{

    public string Title
    {
        get;
        set;
    }
    public Guid Id
    {
        get; set;
    }
    public AppItemModel Item
    {
        get; set;
    }
    public ProjectEditingModel(string title, AppItemModel model)
    {
        Title = title;
        Item = model;
    }

}