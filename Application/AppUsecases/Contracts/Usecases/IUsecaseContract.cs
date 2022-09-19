using System;

using ABI.System;

using Common;

namespace AppUsecases.Contracts.Usecases;
public interface IUsecaseContract<Input, Output> where Input : struct, IComparable, IFormattable, IConvertible, IComparable<Input>, IEquatable<Input>
{
    public Resource<Output> execute(Input parameter);
}
