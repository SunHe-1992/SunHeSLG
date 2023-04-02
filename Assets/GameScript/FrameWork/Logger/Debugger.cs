using System;
using System.Diagnostics;


/**
 * Debug Controle, if unity editor or alpha, don't define DISABLE_DEBUG_LOG, if release just define DISABLE_DEBUG_LOG
 */
//#define DISABLE_DEBUG_LOG
public class Debugger
{
    public static bool LogEnabled = true;
    public static int MainLogLevel = 50;
    public static bool BattleLogEnabled = true;


    public static void Log(object message, UnityEngine.Object obj = null)
    {
#if UNITY_EDITOR || UNITY_WEBGL
        UnityEngine.Debug.Log(message.ToString());
#else
        if (LogEnabled && MainLogLevel >= (int)LogLevel.Log) 
        {
         UnityEngine.Debug.Log(message.ToString());
        }
#endif

    }

    public static void LogWarning(object message, UnityEngine.Object obj = null)
    {
#if UNITY_EDITOR || UNITY_WEBGL
        UnityEngine.Debug.LogWarning(message.ToString());
#else
        if (LogEnabled && MainLogLevel >= (int)LogLevel.Warning)
        {

                  UnityEngine.Debug.LogWarning(message.ToString());
        }
#endif

    }

    public static void LogError(object message, UnityEngine.Object obj = null)
    {

#if UNITY_EDITOR || UNITY_WEBGL
        UnityEngine.Debug.LogError(message.ToString());
#else
        if (LogEnabled && MainLogLevel >= (int)LogLevel.Error)
        {
              UnityEngine.Debug.LogError(message.ToString());
        }
#endif
    }

    public static void LogException(System.Exception e, UnityEngine.Object obj = null)
    {
#if UNITY_EDITOR || UNITY_WEBGL
        UnityEngine.Debug.LogError(e.Message);
#else
        if (LogEnabled && MainLogLevel >= (int)LogLevel.Exception)
        {
        UnityEngine.Debug.LogError(e.Message);
        }
#endif
    }


    public static void LogFormat(string format, params object[] args)
    {
#if UNITY_EDITOR || UNITY_WEBGL
        UnityEngine.Debug.Log(string.Format(format, args));
#else
        if (LogEnabled && MainLogLevel >= (int)LogLevel.Log)
        {
         UnityEngine.Debug.Log(string.Format(format, args));
        }
#endif
    }


    public static void LogWarningFormat(string format, params object[] args)
    {
#if UNITY_EDITOR || UNITY_WEBGL
        UnityEngine.Debug.LogWarning(string.Format(format, args));
#else
        if (LogEnabled && MainLogLevel >= (int)LogLevel.Warning)
        {

             UnityEngine.Debug.LogWarning(string.Format(format, args));
        }
#endif
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
#if UNITY_EDITOR || UNITY_WEBGL
        UnityEngine.Debug.LogWarning(string.Format(format, args));
#else
        if (LogEnabled && MainLogLevel >= (int)LogLevel.Error)
        {
          UnityEngine.Debug.LogWarning(string.Format(format, args));
        }
#endif
    }

    // 记录战斗日志
    public static void LogCombat(string message)
    {
#if !UNITY_WEBGL
        if (BattleLogEnabled)
        {
            Logger.LogCombat(message);
        }
#endif
    }

    public static void LogCombatFormat(string format, params object[] args)
    {
#if !UNITY_WEBGL
        if (BattleLogEnabled)
        {
            Logger.LogCombat(string.Format(format, args));
        }
#endif
    }

    public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        string infoStr = "UnhandledException Sender: ";
        if (sender != null)
        {
            infoStr += sender;
        }
        else
        {
            infoStr += "null, > ";
        }

        if (e != null && e.ExceptionObject != null)
        {
            infoStr += e.ExceptionObject.ToString();
        }

        if (e != null)
        {
            infoStr += "; IsTerminating = ";
            infoStr += e.IsTerminating;
        }
        Debugger.Log(infoStr);
    }

    public static void Print(object obj)
    {
#if UNITY_EDITOR
        Debugger.Log($"print {nameof(obj)} = {obj.ToString()}");
#endif
    }
}

