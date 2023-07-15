﻿using System.ComponentModel;
using System.Reactive;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IEditingItemViewModel : INotifyPropertyChanged
{
    public string ItemName
    {
        get;
    }

    public string Id
    {
        get;
    }

    public bool IsSaved
    {
        get;
    }

    public string ItemPath
    {
        get;
    }

    public IEditingBody Body
    {
        get;
        set;
    }

    public ReactiveCommand<Unit, Unit> CloseItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }
}