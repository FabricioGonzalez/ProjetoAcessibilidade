namespace ProjectAvalonia.Common.Validation;

public interface IRegisterValidationMethod
{
    void RegisterValidationMethod(string propertyName, ValidateMethod validateMethod);
}
