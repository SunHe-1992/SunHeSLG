using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum LoggerType
{
    UNITY_LOGGER,
    FILE_LOGGER,
    CONSOLE_LOGGER,
    BATTLE_LOGGER
}

/// <summary>
/// 日志记录器接口
/// </summary>
public interface ILogger
{
    void LogException(Exception exception, UnityEngine.Object context);
    void LogMessage(LogType logType, string msg);
    void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args);

    LoggerType GetLoggerType();

    string ReportToServer();

    void Close();
}
