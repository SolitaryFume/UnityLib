using System;
using System.Diagnostics;
using UnityEngine;

namespace UnityLib
{
    using Time = UnityEngine.Time;

    [DebuggerDisplay("endFrame:{endFrame}")]
    public class DelayFrameData : IAsync<DelayFrameData>,IAwaiter<DelayFrameData>, IDisposable,IComparable<DelayFrameData>
    {
        public DelayFrameData(){}
        public DelayFrameData(int frame)
        {
            Debug.Assert(frame>0,"frame > 0 !");
            this.endFrame = Time.frameCount+frame;
        }

        public Action callback;
        internal int endFrame;

        public bool IsCompleted => this.endFrame <= Time.frameCount;

        public void Dispose()
        {
            callback = null;
            endFrame = default;
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

        public int CompareTo(DelayFrameData other)
        {
            return this.endFrame - other.endFrame;
        }
    }
}