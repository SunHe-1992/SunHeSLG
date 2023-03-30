using UnityEngine;

namespace RedBjorn.Utils
{
    public abstract class Logger : ILogger
    {
        public string Prefix;

        public void SetPrefix(string prefix)
        {
            Prefix = prefix;
        }

        public void Info(object message)
        {
            Debug.Log(Prefix + message);
        }

        public void Warning(object message)
        {
            Debug.LogWarning(Prefix + message);
        }

        public void Error(object message)
        {
            Debug.LogError(Prefix + message);
        }
    }
}

namespace RedBjorn.Utils.Loggers
{
    public class Global : Logger { }
}