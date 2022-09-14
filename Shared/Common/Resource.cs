namespace Common;
public class Resource<T>
{
    public class Success
    {
        public T? Data
        {
            get; set;
        } = default(T?);
        public Success(T? Data)
        {
            this.Data = Data;
        }

    }
    public class Error
    {
        public string? Message { get; set; } = null;
        public T? Data { get; set; } = default(T?);

        public Error(string? Message,T? Data)
        {
            this.Message = Message;
            this.Data = Data;
        }
    }
    public class IsLoading
    {
        T? Data { get; set; } = default(T?);
        bool isLoading
        {
            get; set;
        } = false;

        public IsLoading(T? Data,bool isLoading)
        {
            this.Data = Data;
            this.isLoading = isLoading;
        }
    }
}
