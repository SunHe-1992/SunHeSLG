namespace RedBjorn.Utils
{
    public interface ILogger
    {
        void SetPrefix(string prefix);
        void Info(object message);
        void Warning(object message);
        void Error(object message);
    }
}
