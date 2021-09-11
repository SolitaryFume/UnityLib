using System;
using UnityEngine;

namespace UnityLib.Time
{
    public class DelayFrameData : IAsync<DelayFrameData>,IAwaiter<DelayFrameData>, IDisposable
    {
        public DelayFrameData(){}
        public DelayFrameData(int frame)
        {
            Debug.Assert(frame>0,"frame > 0 !");
            this.delay = frame;
        }

        public Action callback;
        internal int delay;

        public bool IsCompleted => delay<=0;

        public void Dispose()
        {
            callback = null;
            delay = default;
        }

        public void OnCompleted(Action continuation)
        {
            callback = continuation;
        }

        public DelayFrameData GetAwaiter()
        {
            return this;
        }

        public DelayFrameData GetResult()
        {
            return this;
        }
    }
}