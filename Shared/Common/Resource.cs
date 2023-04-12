namespace Common;

public abstract class Resource<T>
{
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
    )
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
    )
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
    )
    {
        if (resource is Resource<T>.Success success)
        {
            onSuccessAction.Invoke(obj: success);
        }

        return resource;
    }
}