using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public class FileLogger : BaseLogger, ILogger
{

    private string filePath = "";

    private StreamWriter fileWriter;


    private string fileName;

    public FileLogger(string logfile, LogLevel level, bool clearOld,string localStrongName) : base(level)
    {
#if UNITY_EDITOR || UNITY_STANDALONE
            filePath = "";
#else
        filePath = Application.persistentDataPath + "/";
#endif

        fileWriter = null;

        this.fileName = logfile;

        //检查最近的，每12次启动，清理一次日志文件
        int logCounter = LocalStorage.GetIntValue(localStrongName, 0);
        if (logCounter >= 12 || clearOld)  
        {
            File.Delete(filePath + logfile);
            LocalStorage.SaveIntValue(localStrongName, 1);
            logCounter = 1;
        }
        else
        {
            LocalStorage.SaveIntValue(localStrongName, logCounter + 1);
        }

        fileWriter = File.AppendText(filePath + logfile);

        fileWriter.AutoFlush = true;
        this.Write("+++++" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " Loop Counter: " + logCounter + " +++++");
    }

    private bool isBattleLogger = false;

    public void SetBattleLogger(bool flag)
    {
        isBattleLogger = flag;
    }

    // 标记Logger类型
    public LoggerType GetLoggerType()
    {
        if (isBattleLogger)
        {
            return LoggerType.BATTLE_LOGGER;
        }
        else
        {
            return LoggerType.FILE_LOGGER;
        }
    }

    void Write(string content)
    {
        if (fileWriter != null && fileWriter.BaseStream != null && fileWriter.BaseStream.CanWrite)
        {
            fileWriter.WriteLine(content);
        }
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        if (this.IsLogTypeAllowed(LogType.Exception))
            this.Write(Logger.Time + string.Format("[Exception]{0}", exception.ToString()));
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        if (this.IsLogTypeAllowed(logType))
        {
            if (!isBattleLogger)
            {
                this.Write(Logger.Time + "[" + logType.ToString() + "]" + string.Format(format, args));
            }
            else
            {
                this.Write("[" + logType.ToString() + "]" + string.Format(format, args));
            }

        }
    }

    public void LogMessage(LogType logType, string message)
    {
        if (this.IsLogTypeAllowed(logType))
        {
            if (!isBattleLogger)
            {
                this.Write(Logger.Time + message);
            }
            else
            {
                this.Write(message);
            }
        }
    }

    //日志文件上报到服务器
    public string ReportToServer()
    {

        //打开之前先关闭写入
        fileWriter.Flush();
        fileWriter.Close();
        fileWriter = null;

        StreamReader fileReader = new StreamReader(filePath + this.fileName);
        var readFile = "";
        // 上报日志之后，就不再写入后续日志了
        if (fileReader != null)
        {
            if (fileReader.Peek() > -1)
            {
                readFile=fileReader.ReadToEnd();
            }

            fileReader.Close();
            fileReader = null;
        }

        //关闭read然后重开write
        fileWriter = File.AppendText(filePath + this.fileName);
        fileWriter.AutoFlush = true;

        return readFile;
        //FUIManager.Instance.ReportLogFileToServer(filePath + this.fileName);
    }

    public void Close()
    {
        if (fileWriter == null)
            return;

        fileWriter.Flush();
        fileWriter.Close();
        fileWriter = null;
    }
}
