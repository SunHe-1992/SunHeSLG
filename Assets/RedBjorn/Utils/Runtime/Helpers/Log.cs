namespace RedBjorn.Utils
{
    public class Log<T> where T : ILogger, new()
    {
        static T LoggerCached;
        static T Logger
        {
            get
            {
                if (LoggerCached == null)
                {
                    LoggerCached = new T();
                    LoggerCached.SetPrefix(string.Concat("[", typeof(T).Name, "] "));
                }
                return LoggerCached;
            }
        }

        public static void I(object message)
        {
            Logger.Info(message);
        }

        public static void W(object message)
        {
            Logger.Warning(message);
        }

        public static void E(object message)
        {
            Logger.Error(message);
        }
    }
}

namespace RedBjorn
{
    public class Log : Utils.Log<Utils.Loggers.Global> { }
}