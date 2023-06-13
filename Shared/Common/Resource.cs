namespace Common;

public abstract class Resource<T>
{
    public object OnSuccess(Action<object> onSuccessAction) => throw new NotImplementedException();

    public sealed class Success : Resource<T>
    {
        public Success(
            T? Data
        )
        {
            this.Data = Data;
        }

        public T? Data
        {
            get;
            set;
        }
    }

    public sealed class Error : Resource<T>
    {
        public Error(
            string? Message
            , T? Data
        )
        {
            this.Message = Message;
            this.Data = Data;
        }

        public string? Message
        {
            get;
            set;
        }

        public T? Data
        {
            get;
            set;
        }
    }
}

public static class Extensions
{
    public static Resource<T> OnError<T>(
        this Resource<T> resource
        , out T? Data
        , out string? message
    )
    {
        if (resource is Resource<T>.Error error)
        {
            message = error.Message;

            if (error.Data is null)
            {
                Data = default;
            }
            else
            {
                Data = error.Data;
            }
        }
        else
        {
            message = "";
            Data = default;
        }

        return resource;
    }

    public static Resource<T> OnSuccess<T>(
        this Resource<T> resource
        , out T? Data
    ) where T : class
    {
        if (resource is Resource<T>.Success success)
        {
            Data = success.Data;
        }
        else
        {
            Data = default;
        }

        return resource;
    }


    public static Resource<T> OnError<T>(
        this Resource<T> resource
        , Action<Resource<T>.Error> onErrorAction
    ) where T : class
    {
        if (resource is Resource<T>.Error error)
        {
            onErrorAction.Invoke(obj: error);
        }

        return resource;
    }

    public static Resource<T> OnSuccess<T>(
        this Resource<T> resource
        , Action<Resource<T>.Success> onSuccessAction
    ) where T : class
    {
        if (resource is Resource<T>.Success success)
        {
            onSuccessAction.Invoke(obj: success);
        }

        return resource;
    }

    public static (Resource<T> resource, T? value) OnSuccessToTuple<T>(
        this Resource<T> resource
        , Func<Resource<T>.Success, T> onSuccessAction
    ) where T : class
    {
        if (resource is Resource<T>.Success success)
        {
            return ((Resource<T>.Success resource, T? value))(resource, value: onSuccessAction.Invoke(arg: success));
        }

        return (resource, null);
    }

    public static (Resource<T>.Error resource, T? value) OnErrorToTuple<T>(
        this (Resource<T> resource, T? value) resource
        , Func<Resource<T>.Error, T> onErrorAction
    ) where T : class
    {
        if (resource.resource is Resource<T>.Error error)
        {
            return ((Resource<T>.Error resource, T? value))(resource.resource, value: onErrorAction.Invoke(arg: error));
        }

        return ((Resource<T>.Error resource, T? value))resource;
    }

    public static (Resource<T> resource, T? value) OnError<T>(
        this (Resource<T> resource, T? value) resource
        , Func<Resource<T>.Error, T> onErrorAction
    ) where T : class
    {
        if (resource.resource is Resource<T>.Error error)
        {
            return ((Resource<T>.Error resource, T? value))(resource.resource, value: onErrorAction.Invoke(arg: error));
        }

        return resource;
    }


    public static (Resource<T> resource, T? value) OnSuccess<T>(
        this (Resource<T> resource, T? value) resource
        , Func<Resource<T>.Success, T> onSuccessAction
    ) where T : class
    {
        if (resource.resource is Resource<T>.Success success)
        {
            return ((Resource<T>.Success resource, T? value))(resource.resource,
                value: onSuccessAction.Invoke(arg: success));
        }

        return resource with { value = null };
    }
}