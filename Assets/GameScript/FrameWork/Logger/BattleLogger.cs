using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class BattleLogger
{
    private static StreamWriter BattleLogWriter;

    public static void InitLoggerFile()
    {
#if UNITY_EDITOR
        string RootPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        if (!Directory.Exists(RootPath))
        {
            Directory.CreateDirectory(RootPath);
        }
        string BattleLogName = Path.Combine(RootPath, $"mj{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.log");
        BattleLogWriter = File.CreateText(BattleLogName);
#endif
    }

    public static void FlushAndClose()
    {
#if UNITY_EDITOR

        if (BattleLogWriter != null)
        {
            BattleLogWriter.Flush();
            BattleLogWriter.Close();
        }
#endif

    }

    public static void LogCombat(string msg)
    {
#if UNITY_EDITOR

        string writeStr = $"[{DateTime.Now.ToString("HH:mm:ss")}]{msg}";
        if (BattleLogWriter == null)
            return;

        BattleLogWriter.WriteLine(writeStr);
        BattleLogWriter.Flush();
#endif
    }

}
