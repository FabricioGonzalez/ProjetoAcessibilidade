namespace ProjetoAcessibilidade.Contracts.Store;
public interface IStore<T>
{
    public T GetData();
    public void SetData(T data);
}
