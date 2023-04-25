using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Common;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.SearchBar.Settings;
using ProjectAvalonia.Features.Settings.ViewModels;
using ReactiveUI;

namespace ProjectAvalonia.ViewModels.SearchBar.Settings;

public partial class Setting<TTarget, TProperty> : ReactiveObject
{
    private const int SkippedItems = 1;
    [AutoNotify] private TProperty? _value;

    public Setting(
        [DisallowNull] TTarget target
        , Expression<Func<TTarget, TProperty>> selector
    )
    {
        if (target == null)
        {
            throw new ArgumentNullException(paramName: nameof(target));
        }

        if (selector == null)
        {
            throw new ArgumentNullException(paramName: nameof(selector));
        }

        if (PropertyHelper<TTarget>.GetProperty(selector: selector) is not { } pr)
        {
            throw new InvalidOperationException(message: $"The expression {selector} is not a valid property selector");
        }

        Value = (TProperty?)pr.GetValue(obj: target);

        SetValueCommand = ReactiveCommand.Create(execute: () => pr.SetValue(obj: target, value: Value));

        ShowNotificationCommand = ReactiveCommand.Create(execute: () => NotificationHelpers.Show(
            viewModel: new RestartViewModel(
                message: $"To apply the new setting, {Constants.AppName} needs to be restarted")));

        this.WhenAnyValue(property1: x => x.Value)
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Skip(count: SkippedItems)
            .Select(selector: _ => Unit.Default)
            .InvokeCommand(command: SetValueCommand);

        this.WhenAnyValue(property1: x => x.Value)
            .Skip(count: SkippedItems)
            .Throttle(dueTime: TimeSpan.FromMilliseconds(value: SettingsTabViewModelBase.ThrottleTime + 50))
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Where(predicate: _ => SettingsTabViewModelBase.CheckIfRestartIsNeeded())
            .Select(selector: _ => Unit.Default)
            .InvokeCommand(command: ShowNotificationCommand);
    }

    public ICommand SetValueCommand
    {
        get;
    }

    public ICommand ShowNotificationCommand
    {
        get;
    }
}