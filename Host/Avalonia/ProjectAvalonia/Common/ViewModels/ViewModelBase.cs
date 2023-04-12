using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using ProjectAvalonia.Common.Validation;
using ReactiveUI;

namespace ProjectAvalonia.ViewModels;

public class ViewModelBase
    : ReactiveObject
        , INotifyDataErrorInfo
        , IRegisterValidationMethod
{
    private readonly CancellationTokenSource _cancellationToken;
    private readonly Validations _validations;

    protected ObservableAsPropertyHelper<bool> _isBusy = ObservableAsPropertyHelper<bool>.Default();

    public ViewModelBase()
    {
        _cancellationToken = new CancellationTokenSource();
        _validations = new Validations();
        _validations.ErrorsChanged += OnValidations_ErrorsChanged;
        PropertyChanged += ViewModelBase_PropertyChanged;
    }

    public bool IsBusy => _isBusy.Value;

    protected IValidations Validations => _validations;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    bool INotifyDataErrorInfo.HasErrors => Validations.Any;

    IEnumerable INotifyDataErrorInfo.GetErrors(
        string? propertyName
    ) => _validations.GetErrors(propertyName: propertyName);

    void IRegisterValidationMethod.RegisterValidationMethod(
        string propertyName
        , ValidateMethod validateMethod
    ) => ((IRegisterValidationMethod)_validations).RegisterValidationMethod(propertyName: propertyName
        , validateMethod: validateMethod);

    public CancellationToken GetCancellationToken() => _cancellationToken.Token;

    public void CancelCurrentRunningTask()
    {
        if (!_cancellationToken.IsCancellationRequested || _cancellationToken.TryReset())
        {
            _cancellationToken.Cancel();
        }
    }

    protected void ClearValidations() => _validations.Clear();

    private void OnValidations_ErrorsChanged(
        object? sender
        , DataErrorsChangedEventArgs e
    ) => ErrorsChanged?.Invoke(sender: this, e: new DataErrorsChangedEventArgs(propertyName: e.PropertyName));

    private void ViewModelBase_PropertyChanged(
        object? sender
        , PropertyChangedEventArgs e
    )
    {
        if (string.IsNullOrWhiteSpace(value: e.PropertyName))
        {
            _validations.Validate();
        }
        else
        {
            _validations.ValidateProperty(propertyName: e.PropertyName);
        }
    }
}