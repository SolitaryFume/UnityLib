using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityLib
{
    public static class Debug
    {
        [Conditional("Log")]
        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        [Conditional("Log")]
        public static void Log(object message, Object context)
        {
            UnityEngine.Debug.Log(message,context);
        }

        [Conditional("Log")]
        public static void LogFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(format,args:args);
        }

        [Conditional("LogWarning")]
        [Conditional("Log")]
        [Conditional("LogError")]
        public static void LogFormat(Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(context, format, args: args);
        }

        [Conditional("Log")]
        public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(logType, logOptions, context, format,args: args);
        }

        [Conditional("LogWarning")]
        [Conditional("Log")]
        [Conditional("LogError")]
        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
        }

        [Conditional("LogWarning")]
        [Conditional("Log")]
        [Conditional("LogError")]
        public static void LogError(object message, Object context)
        {
            UnityEngine.Debug.LogError(message, context);
        }

        [Conditional("LogWarning")]
        [Conditional("Log")]
        [Conditional("LogError")]
        public static void LogErrorFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(format, args: args);
        }

        [Conditional("LogWarning")]
        [Conditional("Log")]
        [Conditional("LogError")]
        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(context,format, args: args);
        }

        [Conditional("LogWarning")]
        [Conditional("Log")]
        [Conditional("LogError")]
        public static void LogException(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }

        [Conditional("LogWarning")]
        [Conditional("Log")]
        [Conditional("LogError")]
        public static void LogException(Exception exception, Object context)
        {
            UnityEngine.Debug.LogException(exception,context);
        }

        [Conditional("LogWarning")]
        [Conditional("Log")]
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        [Conditional("LogWarning")]
        [Conditional("Log")]
        public static void LogWarning(object message, Object context)
        {
            UnityEngine.Debug.LogWarning(message, context);
        }

        [Conditional("LogWarning")]
        [Conditional("Log")]
        public static void LogWarningFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(format, args: args);
        }

        [Conditional("LogWarning")]
        [Conditional("Log")]
        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(context,format, args: args);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition)
        {
            UnityEngine.Debug.Assert(condition);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, Object context)
        {
            UnityEngine.Debug.Assert(condition, context);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, object message)
        {
            UnityEngine.Debug.Assert(condition, message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, string message)
        {
            UnityEngine.Debug.Assert(condition, message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, object message, Object context)
        {
            UnityEngine.Debug.Assert(condition, message, context);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, string message, Object context)
        {
            UnityEngine.Debug.Assert(condition, message, context);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertFormat(bool condition, string format, params object[] args)
        {
            UnityEngine.Debug.AssertFormat(condition, format, args: args);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertFormat(bool condition, Object context, string format, params object[] args)
        {
            UnityEngine.Debug.AssertFormat(condition, context,format, args: args);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void LogAssertion(object message)
        {
            UnityEngine.Debug.LogAssertion(message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void LogAssertion(object message, Object context)
        {
            UnityEngine.Debug.LogAssertion(message, context);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void LogAssertionFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogAssertionFormat(format, args: args);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void LogAssertionFormat(Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogAssertionFormat(context,format, args: args);
        }
    }
}
