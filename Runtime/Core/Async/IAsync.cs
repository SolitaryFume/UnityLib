using System;
using System.Runtime.CompilerServices;

namespace UnityLib
{

    public interface IAsync<T> : INotifyCompletion
    {
        T GetResult();
        bool IsCompleted { get; }
    }

}