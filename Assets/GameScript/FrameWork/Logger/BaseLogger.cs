using UnityEngine;
using System.Collections;
using System;

public class BaseLogger
{
    //private int logFilter;
    private LogLevel logLevel;
    public BaseLogger(LogLevel level)
    {
        this.logLevel = level;
    }

    public bool IsLogTypeAllowed(LogType logType)
    {
        if (logType == LogType.Exception)
            return true;

        if ((int)logLevel < (int)logType)
            return false;
       
        return true;
    }
    
}
