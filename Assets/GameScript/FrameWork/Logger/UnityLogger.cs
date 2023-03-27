
using UnityEngine;
using System;

public class UnityLogger : BaseLogger, ILogger
{

    private LogLevel logLevel;
    public UnityLogger(LogLevel level, int filter) : base(level)
    {
    }

    public LoggerType GetLoggerType()
    {
        return LoggerType.UNITY_LOGGER;
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        if (this.IsLogTypeAllowed(LogType.Exception))
        {
            Debug.unityLogger.LogException(exception, context);
        }
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        if (this.IsLogTypeAllowed(logType))
        {
            Debug.unityLogger.LogFormat(logType, context, format, args);
        }
    }

    public void LogError( UnityEngine.Object context, string format, params object[] args)
    {
        if (this.IsLogTypeAllowed(LogType.Error))
        {
            Debug.unityLogger.LogError(LogType.Error.ToString(),context);
        }
    }

    public void Log(UnityEngine.Object context, string format, params object[] args)
    {
        if (this.IsLogTypeAllowed(LogType.Log))
        {
            Debug.unityLogger.Log(LogType.Log.ToString(), context);
        }
    }

    public string ReportToServer()
    {
        return "";
    }
    
    public void Close()
    {
        
    }

    public void LogMessage(LogType logType, string msg)
    {
       
    }
}
