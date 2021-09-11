using System;
using System.Runtime.CompilerServices;

public interface IAsync<T>:INotifyCompletion
{
    T GetResult();
    bool IsCompleted { get; }
}
