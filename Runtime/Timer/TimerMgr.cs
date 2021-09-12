using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityLib.Pool;

namespace UnityLib.Time
{
    using Time = UnityEngine.Time;

    public class TimerMgr : MonoBehaviour
    {
        private List<DelayTimeData> m_timeList;
        private List<DelayFrameData> m_frameList;
        private DisposableObjectPool<DelayTimeData> m_timeDataPool = new DisposableObjectPool<DelayTimeData>(new DefaultPooledObjectPolicy<DelayTimeData>());
        private DisposableObjectPool<DelayFrameData> m_frameDataPool = new DisposableObjectPool<DelayFrameData>(new DefaultPooledObjectPolicy<DelayFrameData>());

        private static TimerMgr _instance;
        private static TimerMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject("[TimerMgr]", typeof(TimerMgr));
                    DontDestroyOnLoad(obj);
                    _instance = obj.GetComponent<TimerMgr>();
                }
                return _instance;
            }
        }

        private static DelayTimeData GetDelayTimeData() =>Instance.m_timeDataPool.Get();
        private static DelayFrameData GetDelayFrameData() => Instance.m_frameDataPool.Get();

        private void Awake()
        {
            m_timeList = new List<DelayTimeData>(10);
            m_frameList = new List<DelayFrameData>(10);
            StartCoroutine(nameof(Loop));
        }

        private void OnDestroy()
        {
            StopCoroutine(nameof(Loop));
        }
        private IEnumerator Loop()
        {
            while (true)
            {
                TimeLoop();
                FrameLoop();
                yield return null;
            }
        }

        private void TimeLoop()
        {
            var time = Time.time;
            while (m_timeList.Count>0 && m_timeList[0].endTime<=time)
            {
                m_timeList[0].callback?.Invoke();
                m_timeList.RemoveAt(0);
            }
        }

        private void FrameLoop()
        {
            var frameCount = Time.frameCount;
            while (m_frameList.Count > 0 && m_frameList[0].endFrame <= frameCount)
            {
                m_frameList[0].callback?.Invoke();
                m_frameList.RemoveAt(0);
            }
        }

        public static DelayTimeData DelayTime(float second)
        {
            if (second<=0)
                throw new ArgumentOutOfRangeException(nameof(second));
            var temp = GetDelayTimeData();
            temp.endTime = UnityEngine.Time.time + second;
            TimerMgr.Instance.m_timeList.SoltAdd(temp);
            return temp;
        }

        public static DelayTimeData DelayTime(TimeSpan span)
        {
            return DelayTime((float)span.TotalSeconds);
        }

        public static DelayFrameData DelayFrame(int frame)
        {
            if (frame <= 0)
                throw new ArgumentOutOfRangeException(nameof(frame));
            var temp = GetDelayFrameData();
            temp.endFrame = Time.frameCount + frame;
            TimerMgr.Instance.m_frameList.SoltAdd(temp);
            return temp;
        }

        public static void StartDelay(DelayTimeData data)
        {
            if(data==null)
                throw new ArgumentNullException(nameof(data));
            TimerMgr.Instance.m_timeList.SoltAdd(data);
        }

        public static void StartDelay(DelayFrameData data)
        {
            if(data==null)
                throw new ArgumentNullException(nameof(data));
            TimerMgr.Instance.m_frameList.SoltAdd(data);
        }

        public static void StopDelay(DelayTimeData data)
        {
            if(data==null)
                throw new ArgumentNullException(nameof(data));
            TimerMgr.Instance.m_timeList.Remove(data);
        }

        public static void StopDelay(DelayFrameData data)
        {
            if(data==null)
                throw new ArgumentNullException(nameof(data));
            TimerMgr.Instance.m_frameList.Remove(data);
        }
    }
}