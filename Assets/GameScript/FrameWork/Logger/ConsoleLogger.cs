using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.IO;

public class ConsoleLogger : BaseLogger,ILogger
{

    public ConsoleLogger(LogLevel level, int filter) : base(level)
    {
        Initialize();
    }

    ~ConsoleLogger()
    {
        Shutdown();
    }

    public LoggerType GetLoggerType()
    {
        return LoggerType.CONSOLE_LOGGER;
    }
    
    public void LogException(Exception exception, UnityEngine.Object context)
    {
        if (this.IsLogTypeAllowed(LogType.Exception))
            Console.WriteLine(string.Format("[Exception]{0})", exception.ToString()));
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        if (this.IsLogTypeAllowed(logType))
            Console.WriteLine(Logger.Time + "[" + logType.ToString() + "]" + string.Format(format, args));
    }

    public void LogMessage(LogType logType, string message)
    {
        if(this.IsLogTypeAllowed(logType))
        {
            Console.WriteLine(message);
        }
    }

    
    TextWriter oldOutput;
    static FileStream fileStream;
    static StreamWriter standardOutput;

    public void Initialize()
    {
        if (!AttachConsole(-1))
        {
            AllocConsole();
            Console.WriteLine("Console Alloced");
        }
        else
        {
            Console.WriteLine("Console Initialized");
        }

        oldOutput = Console.Out;

        try
        {
            IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            fileStream = new FileStream(stdHandle, FileAccess.Write);
            standardOutput = new StreamWriter(fileStream, System.Text.Encoding.ASCII);
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
        }
        catch (System.Exception e)
        {
            Debugger.Log("Couldn't redirect output: " + e.Message);
        }
    }

    public void Shutdown()
    {
        Console.SetOut(oldOutput);
        FreeConsole();
    }

    public void SetTitle(string strName)
    {
        SetConsoleTitle(strName);
    }

    public string ReportToServer()
    {
        return "";
    }

    public void Close()
    {
        
    }

    private const int STD_OUTPUT_HANDLE = -11;

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool AttachConsole(int dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool FreeConsole();

    [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    static extern bool SetConsoleTitle(string lpConsoleTitle);
}
