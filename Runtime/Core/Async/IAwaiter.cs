namespace UnityLib
{

    public interface IAwaiter<T>
    {
        T GetAwaiter();

    }
}