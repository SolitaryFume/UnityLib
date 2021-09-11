using System;
using UnityEngine;

namespace UnityLib.Time
{
    public class DelayTimeData : IAsync<DelayTimeData>,IAwaiter<DelayTimeData>,IDisposable
    {
        public DelayTimeData(){}
        public DelayTimeData(float delay)
        {
            Debug.Assert(delay>0,"frame > 0 !");
            this.delay = delay;
        }
        internal float delay = default;

        public Action callback;
        public bool IsCompleted => delay <= 0;
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
            this.delay = default;
        }
    }
}