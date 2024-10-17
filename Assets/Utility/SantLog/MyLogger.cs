using System;
using UnityEngine;

namespace SantLog
{
    internal class MyLogger : ILogger
    {
        public ILogHandler logHandler { get; set; } = new SimpleLogHandler();
        public bool logEnabled { get; set; } = true;
        public LogType filterLogType { get; set; } = LogType.Log;

        public bool IsLogTypeAllowed(LogType logType)
        {
            return logEnabled && (logType <= filterLogType);
        }

        public void Log(LogType logType, object message)
        {
            if (IsLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, null, "{0}", message);
            }
        }

        public void Log(LogType logType, object message, UnityEngine.Object context)
        {
            if (IsLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, context, "{0}", message);
            }
        }

        public void Log(LogType logType, string tag, object message)
        {
            if (IsLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, null, "[{0}] {1}", tag, message);
            }
        }

        public void Log(LogType logType, string tag, object message, UnityEngine.Object context)
        {
            if (IsLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, context, "[{0}] {1}", tag, message);
            }
        }

        public void Log(object message)
        {
            Log(LogType.Log, message);
        }

        public void Log(string tag, object message)
        {
            Log(LogType.Log, tag, message);
        }

        public void Log(string tag, object message, UnityEngine.Object context)
        {
            Log(LogType.Log, tag, message, context);
        }

        public void LogError(string tag, object message)
        {
            Log(LogType.Error, tag, message);
        }

        public void LogError(string tag, object message, UnityEngine.Object context)
        {
            Log(LogType.Error, tag, message, context);
        }

        public void LogException(Exception exception)
        {
            if (logEnabled && LogType.Exception <= filterLogType)
            {
                logHandler.LogException(exception, null);
            }
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            if (logEnabled && LogType.Exception <= filterLogType)
            {
                logHandler.LogException(exception, context);
            }
        }

        public void LogFormat(LogType logType, string format, params object[] args)
        {
            if (IsLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, null, format, args);
            }
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            if (IsLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, context, format, args);
            }
        }

        public void LogWarning(string tag, object message)
        {
            Log(LogType.Warning, tag, message);
        }

        public void LogWarning(string tag, object message, UnityEngine.Object context)
        {
            Log(LogType.Warning, tag, message, context);
        }
    }

}
