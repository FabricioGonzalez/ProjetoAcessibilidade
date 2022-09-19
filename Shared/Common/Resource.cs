namespace Common;
public  class Resource<T>
{
    public sealed class Success : Resource<T>
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
    public sealed class Error : Resource<T>
    {
        public string? Message { get; set; } = null;
        public T? Data { get; set; } = default(T?);

        public Error(string? Message,T? Data)
        {
            this.Message = Message;
            this.Data = Data;
        }
    }
    public sealed class IsLoading : Resource<T>
    {
        public T? Data { get; set; } = default(T?);
        public bool isLoading
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
