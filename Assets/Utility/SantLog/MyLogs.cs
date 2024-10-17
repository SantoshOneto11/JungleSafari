namespace SantLog
{
    public class MyLogs
    {
        private static MyLogger myLogger = new MyLogger();
        private static bool isLoggerEnabled = false;

        public static void EnableLogging(bool enable)
        {
            isLoggerEnabled = enable;
            myLogger.logEnabled = enable;
        }

        public static void Log(string message)
        {
            if (isLoggerEnabled)
            {
                myLogger.Log(message);
            }
        }

        public static void LogError(string message)
        {
            if (isLoggerEnabled)
            {
                myLogger.LogError("Error", message);
            }
        }

        public static void LogWarning(string message)
        {
            if (isLoggerEnabled)
            {
                myLogger.LogWarning("Warning", message);
            }
        }

        public static void LogException(System.Exception exception)
        {
            if (isLoggerEnabled)
            {
                myLogger.LogException(exception);
            }
        }
    }
}

