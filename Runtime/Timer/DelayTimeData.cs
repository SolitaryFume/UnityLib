using System;
using UnityEngine;
using System.Diagnostics;

namespace UnityLib.Time
{

    [DebuggerDisplay("endTime:{endTime}")]
    public class DelayTimeData : IAsync<DelayTimeData>,IAwaiter<DelayTimeData>,IDisposable,IComparable<DelayTimeData>
    {
        public DelayTimeData(){}
        public DelayTimeData(float delay)
        {
            Debug.Assert(delay>0,"frame > 0 !");
            this.endTime = UnityEngine.Time.time+delay;
        }
        internal float endTime = default;

        public Action callback;
        public bool IsCompleted => UnityEngine.Time.time>=endTime;
        public DelayTimeData GetResult()=> this;

        public void OnCompleted(Action continuation)
        {
            callback = continuation;
        }

        public DelayTimeData GetAwaiter()
        {
            return this;
        }

        public void Dispose()
        {
            this.callback = null;
            this.endTime = default;
        }

        public int CompareTo(DelayTimeData other)
        {
            return (int)( this.endTime - other.endTime);
        }
    }
}