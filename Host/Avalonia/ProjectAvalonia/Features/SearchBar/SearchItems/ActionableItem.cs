using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjectAvalonia.Features.SearchBar.Patterns;
using ReactiveUI;

namespace ProjectAvalonia.Features.SearchBar.SearchItems;

public class ActionableItem : IActionableItem
{
    public ActionableItem(
        string name
        , string description
        , Func<Task> onExecution
        , string category
        , IEnumerable<string>? keywords = null
    )
    {
        Name = name;
        Description = description;
        OnExecution = onExecution;
        Category = category;
        Keywords = keywords ?? Enumerable.Empty<string>();
        Command = ReactiveCommand.CreateFromTask(execute: onExecution);
    }

    public ICommand Command
    {
        get;
        set;
    }

    public Func<Task> OnExecution
    {
        get;
    }

    public string Name
    {
        get;
    }

    public string Description
    {
        get;
    }

    public ComposedKey Key => new(keys: Name);

    public string? Icon
    {
        get;
        set;
    }

    public string Category
    {
        get;
    }

    public IEnumerable<string> Keywords
    {
        get;
    }

    public bool IsDefault
    {
        get;
        set;
    }
}