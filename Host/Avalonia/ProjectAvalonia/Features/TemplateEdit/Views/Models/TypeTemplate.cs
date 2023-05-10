using System;
using Avalonia.Controls;

namespace ProjectAvalonia.Features.TemplateEdit.Views.Models;

public class TypeTemplate
{
    public Enum TypeName
    {
        get;
        set;
    }

    public IControl UIElement
    {
        get;
        set;
    }
}