using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityLib.Pool;

namespace UnityLib.Time
{
    public class TimerMgr : MonoBehaviour
    {
        private LinkedList<DelayTimeData> m_linkTimeList;
        private LinkedList<DelayFrameData> m_linkFrameList;
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
            m_linkTimeList = new LinkedList<DelayTimeData>();
            m_linkFrameList = new LinkedList<DelayFrameData>();
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
                var dataTime = UnityEngine.Time.deltaTime;

                //timeloop
                var timeNode = m_linkTimeList.First;
                while (timeNode != null)
                {
                    var next = timeNode.Next;
                    var data = timeNode.Value;
                    data.delay-=dataTime;
                    if (data.delay <= 0)
                    {
                        data.callback?.Invoke();
                        m_linkTimeList.Remove(timeNode);
                        m_timeDataPool.Return(data);
                    }
                    timeNode = next;
                }

                //frameloop
                var frameNode = m_linkFrameList.First;
                while(frameNode!=null)
                {
                    var next = frameNode.Next;
                    var data = frameNode.Value;
                    data.delay--;
                    if(data.delay<=0)
                    {
                        data.callback?.Invoke();
                        m_linkFrameList.Remove(frameNode);
                        m_frameDataPool.Return(data);
                    }
                    frameNode = next;
                }

                yield return null;
            }
        }

        public static DelayTimeData DelayTime(float second)
        {
            var temp = GetDelayTimeData();
            temp.delay = second;
            TimerMgr.Instance.m_linkTimeList.AddLast(temp);
            return temp;
        }

        public static DelayTimeData DelayTime(TimeSpan span)
        {
            return DelayTime((float)span.TotalSeconds);
        }

        public static DelayFrameData DelayFrame(int frame)
        {
            var temp = GetDelayFrameData();
            temp.delay = frame;
            TimerMgr.Instance.m_linkFrameList.AddLast(temp);
            return temp;
        }

        public static void StartDelay(DelayTimeData data)
        {
            if(data==null)
                throw new ArgumentNullException(nameof(data));
            TimerMgr.Instance.m_linkTimeList.AddLast(data);
        }

        public static void StartDelay(DelayFrameData data)
        {
            if(data==null)
                throw new ArgumentNullException(nameof(data));
            TimerMgr.Instance.m_linkFrameList.AddLast(data);
        }

        public static void StopDelay(DelayTimeData data)
        {
            if(data==null)
                throw new ArgumentNullException(nameof(data));
            TimerMgr.Instance.m_linkTimeList.AddLast(data);
        }

        public static void StopDelay(DelayFrameData data)
        {
            if(data==null)
                throw new ArgumentNullException(nameof(data));
            TimerMgr.Instance.m_linkFrameList.Remove(data);
        }
    }
}