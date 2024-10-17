using System;
using UnityEngine;
namespace SantLog
{
    public class SimpleLogHandler : ILogHandler
    {
        public void LogException(Exception exception, UnityEngine.Object context)
        {
            // Log the exception message without the stack trace
            Debug.LogError(exception.Message);
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            // Only log the formatted message without the extra context
            string message = string.Format(format, args);
            Debug.Log(message); // You can customize how it logs (e.g., write to a file)
        }
    }
}
