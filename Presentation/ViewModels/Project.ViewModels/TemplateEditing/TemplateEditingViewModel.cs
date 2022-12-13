﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppViewModels.Common;

using ReactiveUI;

namespace AppViewModels.TemplateEditing;
public class TemplateEditingViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen
    {
        get; set;
    }
    public string UrlPathSegment { get; } = "TemplateEditing";

    public TemplateEditingViewModel()
    {

    }
}