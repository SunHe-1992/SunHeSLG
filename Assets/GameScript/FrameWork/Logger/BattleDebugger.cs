using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDebugger
{
    public static bool LogEnabled = false;
    public static int MainLogLevel = 1;

    public static void Log(string log)
    {
        Debugger.Log(log);
    }

    public static void LogError(string error)
    {
        Debug.Log(error);
    }

    public static void LogWarning(string warn)
    {
        Debugger.LogWarning(warn);
    }

}
