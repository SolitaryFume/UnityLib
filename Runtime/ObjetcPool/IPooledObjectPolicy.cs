namespace UnityLib.Pool
{
    public interface IPooledObjectPolicy<T>
    {
        T Create();
        bool Return(T obj);
    }
}