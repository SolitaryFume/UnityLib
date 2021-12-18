using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Layout.Pattern;
using log4net.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using static log4net.Appender.FileAppender;
using Object = UnityEngine.Object;

namespace UnityLib
{
    public static class Debug
    {
        //[Conditional("Log")]
        [DebuggerHidden]
        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        [Conditional("Log")]
        [DebuggerHidden]
        public static void Log(object message, Object context)
        {
            UnityEngine.Debug.Log(message, context);
        }

        [Conditional("Log")]
        [DebuggerHidden]
        public static void LogFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(format, args: args);
        }

        [Conditional("Log")]
        [DebuggerHidden]
        public static void LogFormat(Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(context, format, args: args);
        }

        [Conditional("Log")]
        [DebuggerHidden]
        public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args: args);
        }

        //[Conditional("Log")]
        //[Conditional("LogWarning")]
        //[Conditional("LogError")]
        [DebuggerHidden]
        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
        }

        [Conditional("Log")]
        [Conditional("LogWarning")]
        [Conditional("LogError")]
        [DebuggerHidden]
        public static void LogError(object message, Object context)
        {
            UnityEngine.Debug.LogError(message, context);
        }

        [Conditional("Log")]
        [Conditional("LogWarning")]
        [Conditional("LogError")]
        [DebuggerHidden]
        public static void LogErrorFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(format, args: args);
        }

        [Conditional("Log")]
        [Conditional("LogWarning")]
        [Conditional("LogError")]
        [DebuggerHidden]
        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(context, format, args: args);
        }

        [Conditional("Log")]
        [Conditional("LogWarning")]
        [Conditional("LogError")]
        [DebuggerHidden]
        public static void LogException(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }

        [Conditional("Log")]
        [Conditional("LogWarning")]
        [Conditional("LogError")]
        [DebuggerHidden]
        public static void LogException(Exception exception, Object context)
        {
            UnityEngine.Debug.LogException(exception, context);
        }

        [Conditional("Log")]
        [Conditional("LogWarning")]
        [DebuggerHidden]
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        [Conditional("Log")]
        [Conditional("LogWarning")]
        [DebuggerHidden]
        public static void LogWarning(object message, Object context)
        {
            UnityEngine.Debug.LogWarning(message, context);
        }

        [Conditional("Log")]
        [Conditional("LogWarning")]
        [DebuggerHidden]
        public static void LogWarningFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(format, args: args);
        }

        [Conditional("Log")]
        [Conditional("LogWarning")]
        [DebuggerHidden]
        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(context, format, args: args);
        }

        [Conditional("UNITY_ASSERTIONS")]
        [DebuggerHidden]
        public static void Assert(bool condition)
        {
            UnityEngine.Debug.Assert(condition);
        }

        [Conditional("UNITY_ASSERTIONS")]
        [DebuggerHidden]
        public static void Assert(bool condition, Object context)
        {
            UnityEngine.Debug.Assert(condition, context);
        }

        [Conditional("UNITY_ASSERTIONS")]
        [DebuggerHidden]
        public static void Assert(bool condition, object message)
        {
            UnityEngine.Debug.Assert(condition, message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        [DebuggerHidden]
        public static void Assert(bool condition, string message)
        {
            UnityEngine.Debug.Assert(condition, message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        [DebuggerHidden]
        public static void Assert(bool condition, object message, Object context)
        {
            UnityEngine.Debug.Assert(condition, message, context);
        }

        [Conditional("UNITY_ASSERTIONS")]
        [DebuggerHidden]
        public static void Assert(bool condition, string message, Object context)
        {
            UnityEngine.Debug.Assert(condition, message, context);
        }

        [Conditional("UNITY_ASSERTIONS")]
        [DebuggerHidden]
        public static void AssertFormat(bool condition, string format, params object[] args)
        {
            UnityEngine.Debug.AssertFormat(condition, format, args: args);
        }

        [Conditional("UNITY_ASSERTIONS")]
        [DebuggerHidden]
        public static void AssertFormat(bool condition, Object context, string format, params object[] args)
        {
            UnityEngine.Debug.AssertFormat(condition, context, format, args: args);
        }

        [Conditional("UNITY_ASSERTIONS")]
        [DebuggerHidden]
        public static void LogAssertion(object message)
        {
            UnityEngine.Debug.LogAssertion(message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        [DebuggerHidden]
        public static void LogAssertion(object message, Object context)
        {
            UnityEngine.Debug.LogAssertion(message, context);
        }

        [Conditional("UNITY_ASSERTIONS")]
        [DebuggerHidden]
        public static void LogAssertionFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogAssertionFormat(format, args: args);
        }

        [Conditional("UNITY_ASSERTIONS")]
        [DebuggerHidden]
        public static void LogAssertionFormat(Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogAssertionFormat(context, format, args: args);
        }
    }

    public class LogUtility
    {
        public static ILog log;

        static LogUtility()
        {
            ILoggerRepository repository = log4net.LogManager.GetRepository();
            FileAppender fileAppender = new FileAppender();
            fileAppender.Name = "LogFileAppender";
            fileAppender.File = @"C:\Users\admin\Desktop\Log\Test.log";
            fileAppender.AppendToFile = true;
            fileAppender.LockingModel = new MinimalLock();
            PatternLayout patternLayout = new PatternLayout
            {
                ConversionPattern = "[%d{hh:mm:ss}] [%thread] [%-5level]>>>%message%newline"
            };
            //patternLayout.AddConverter("stack", typeof(StackTraceConverter));
            patternLayout.ActivateOptions();
            fileAppender.Layout = patternLayout;
            fileAppender.Encoding = Encoding.UTF8;
            fileAppender.ActivateOptions();

            BasicConfigurator.Configure(repository, fileAppender);
            log = LogManager.GetLogger(repository.Name);
        }
    }

    public class CustomPatternLayout : PatternLayout
    {
        private const string Key_Stack = "stack";

        public CustomPatternLayout()
        {
            this.AddConverter(Key_Stack,typeof(StackTraceConverter));
        }
    }

    public class StackTraceConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var stack = new StackTrace(13,true);
            writer.Write(stack);
        }
    }
}
