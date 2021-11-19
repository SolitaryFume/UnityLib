using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace UnityLib
{
#if UNITY_EDITOR
    internal static class CoroutineList
    {
        public static void Delete(this List<CoroutineInfo> list, Coroutine coroutine)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (coroutine == null)
                throw new ArgumentNullException(nameof(coroutine));
            for (int i = 0; i < list.Count; i++)
            {
                var info = list[i];
                if (info != null && info.Coroutine == coroutine)
                {
                    list.RemoveAt(i);
                    return;
                }
            }
        }
    }

    [System.Serializable]
    public class CoroutineInfo
    {
        public string CrateTime;
        public string[] Trace;
        public Coroutine Coroutine;
    }

#endif

    public class CoroutineMgr:MonoBehaviour
    {
        private static CoroutineMgr _instance;
        private static CoroutineMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject("[CoroutineMgr]", typeof(CoroutineMgr));
                    DontDestroyOnLoad(obj);
                    _instance = obj.GetComponent<CoroutineMgr>();
                }
                return _instance;
            }
        }
        
        public static Coroutine AddCoroutine(IEnumerator routine)
        {
            var result = Instance.StartCoroutine(routine);
            #if UNITY_EDITOR 
            var debugInfo =new CoroutineInfo(){
                CrateTime = DateTime.Now.ToString(),
                Trace = new StackTrace(true).ToString().Split('\n'),
                Coroutine = result
            };
            Instance.list.Add(debugInfo);
            #endif
            UnityEngine.Debug.Log("添加");
            return result;
        }

        public static void RemoveCoroutine(Coroutine routine)
        {
            #if UNITY_EDITOR
            Instance.list.Delete(routine);
            #endif
            Instance.StopCoroutine(routine);
        }

        public static Coroutine InvokeRepeating(Action action,float time,float repeatRate)
        {
            if(action==null)
                throw new ArgumentNullException(nameof(action));
            if(time<0) 
                throw new ArgumentOutOfRangeException(nameof(time));
            if (repeatRate <= 0)
                throw new ArgumentOutOfRangeException(nameof(repeatRate));

            IEnumerator leep(Action action,float time,float repeatRate)
            { 
                yield return new WaitForSeconds(time);
                action();
                var repeatRateWait = new WaitForSeconds(repeatRate);

                while (true)
                {
                    yield return repeatRateWait;
                    action();
                }
            }

            return AddCoroutine(leep(action, time, repeatRate));
        }

#if UNITY_EDITOR
        [SerializeField] private List<CoroutineInfo> list = new List<CoroutineInfo>(20);
        #endif
    }

}