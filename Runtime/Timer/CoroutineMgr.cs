using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace UnityLib
{
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

        #if UNITY_EDITOR
        [SerializeField] private List<CoroutineInfo> list = new List<CoroutineInfo>(20);
        #endif
    }

}