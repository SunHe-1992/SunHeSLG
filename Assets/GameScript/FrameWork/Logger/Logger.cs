using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

public enum LogLevel
{
    Exception = 0,
    Error = 1,
    Assert = 2,
    Warning = 3,
    Log = 4,
    Info = 5,
}


class LogHandler : ILogHandler
{
        public void LogException(Exception exception, UnityEngine.Object context)
    {
        Logger.LogException(exception, context);
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        Logger.LogFormat(logType, context, format, args);
    }
  
}

public class LogConfig
{
    public string Name;
    public string Type;
    public bool Enable;
    public LogLevel LogLevel;
    public int LogFilter;
    public string LogFile;
}

public class Logger
{
    static int frame = 0;
    private static List<ILogger> loggers = new List<ILogger>();

    public static void Init(List<LogConfig> configs)
    {
        Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;

        loggers.Clear();
        foreach (LogConfig config in configs)
        {
            if (!config.Enable)
            {
                continue;
            }

            if (config.Type == "Unity")
            {
#if UNITY_EDITOR
                Logger.AppendLogger(new UnityLogger(config.LogLevel, config.LogFilter));
#endif
                return;
            }


            switch (config.Type)
            {
                case "File":
                    FileLogger fileLogger = new FileLogger(config.LogFile, config.LogLevel, false,LocalStorage.CLEAR_LOG_COUNTER);
                    Logger.AppendLogger(fileLogger);
                    break;

                case "Battle":
                    FileLogger battleLogger = new FileLogger(config.LogFile, config.LogLevel, true, LocalStorage.CLEAR_BATTLELOG_COUNTER);
                    battleLogger.SetBattleLogger(true);
                    Logger.AppendLogger(battleLogger);
                    break;

                case "Console":
#if UNITY_STANDALONE
                    Logger.AppendLogger(new ConsoleLogger(config.LogLevel, config.LogFilter));
#endif
                    break;
            }
        }
    }

    public static void Close()
    {
        foreach (ILogger logger in loggers)
        {
            logger.Close();
        }
    }

    public static string Time
    {
        get
        {
            if (Application.isPlaying)
            {
                return DateTime.Now.ToString("[HH:mm:ss.ffffff]:");
            }
            else
            {
                return "";
            }

        }
    }


    // 
    private static void Application_logMessageReceivedThreaded(string condition, string stackTrace, LogType type)
    {
#if UNITY_EDITOR

        if (!string.IsNullOrEmpty(condition) && condition.IndexOf("[UnityLogger]") >= 0) {
            return;
        }

#else
        if (GlobalConfig.LogEnabled)
        {
            if (type == LogType.Exception)
            {
                try
                {
                    for (int i = 0; i < loggers.Count; i++)
                    {
                        loggers[i].LogFormat(LogType.Error, null, "{0}\r\nstackTrace:\r\n{1}", "Exception Received: " + condition, stackTrace);
                    }
                }
                catch (Exception e)
                {
                    // Catch Any LogFormat caused Exception, to avoid dead loop in this case
                }
            }
        }
#endif
    }

    public static void AppendLogger(ILogger logger)
    {
        loggers.Add(logger);
    }


    public static void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        if (Debugger.LogEnabled)
        {
            string logInfo = logType.ToString();
            string logContent = logInfo + string.Format(format, args);

            for (int i = 0; i < loggers.Count; i++)
            {
                ILogger logger = loggers[i];
                if (logger == null || logger.GetLoggerType() == LoggerType.BATTLE_LOGGER) continue;
                logger.LogMessage(logType, logContent);
            }
        }
    }

    public static string ReportLogToServer(int loggerTypeNum)
    {
        if (loggers == null || loggers.Count <= 0)
        {
            return "";
        }

        string reportLog = "";
        for (int i = 0; i < loggers.Count; i++)
        {
            ILogger logger = loggers[i];
            LoggerType loggerType = logger.GetLoggerType();
            if (loggerType == (LoggerType)loggerTypeNum)
            {
                reportLog = reportLog + "\n" + logger.ReportToServer();
            }
        }

        //上报之后，把记录数量增加，下次启动会清理掉老的日志文件，同时需要清空出现过错误的标记
        // LocalStorage.SaveIntValue(LocalStorage.HAVE_ERROR_TO_REPORT, 0);
        LocalStorage.SaveIntValue(LocalStorage.CLEAR_LOG_COUNTER, 50);

        return reportLog;
    }

    public static void LogException(Exception exception, UnityEngine.Object context)
    {
        if (Debugger.LogEnabled)
        {
            for (int i = 0; i < loggers.Count; i++)
            {
                ILogger logger = loggers[i];
                if (logger == null || logger.GetLoggerType() == LoggerType.BATTLE_LOGGER) continue;
                logger.LogException(exception, context);
            }
        }
    }

    public static void Log(string log, UnityEngine.Object obj = null)
    {
        if (Debugger.LogEnabled)
        {
            for (int i = 0; i < loggers.Count; i++)
            {
                ILogger logger = loggers[i];
                if (logger == null || logger.GetLoggerType() == LoggerType.BATTLE_LOGGER) continue;
                logger.LogMessage(LogType.Log, log);
            }
        }
    }

    public static void LogWarning(string log, UnityEngine.Object obj = null)
    {
        if (Debugger.LogEnabled)
        {
            for (int i = 0; i < loggers.Count; i++)
            {
                ILogger logger = loggers[i];
                if (logger == null || logger.GetLoggerType() == LoggerType.BATTLE_LOGGER) continue;
                logger.LogMessage(LogType.Warning, log);
            }
        }
    }

    public static void LogError(string log, UnityEngine.Object obj = null)
    {
        if (Debugger.LogEnabled)
        {
            for (int i = 0; i < loggers.Count; i++)
            {
                ILogger logger = loggers[i];
                if (logger == null || logger.GetLoggerType() == LoggerType.BATTLE_LOGGER) continue;
                logger.LogMessage(LogType.Error, log);
            }
        }
    }


    public static void LogCombat(string log)
    {
        if (Debugger.BattleLogEnabled)
        {
            for (int i = 0; i < loggers.Count; i++)
            {
                ILogger logger = loggers[i];

                if (logger != null && logger.GetLoggerType() == LoggerType.BATTLE_LOGGER)
                {
                    logger.LogMessage(LogType.Exception, log);
                }
            }
        }
    }

    public static string GetLogs(bool combat = false)
    {
        return "";
    }

    public static void Error(int error)
    {

    }

    public static void PushToServer()
    {

    }
}
