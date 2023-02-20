﻿using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.FormItemState;
public class OptionsItemState : ReactiveObject
{
    private string value;
    public string Value
    {
        get => value;
        set => this.RaiseAndSetIfChanged(ref this.value, value);
    }

    private bool isChecked = false;
    public bool IsChecked
    {
        get => isChecked;
        set => this.RaiseAndSetIfChanged(ref isChecked, value);
    }
}
