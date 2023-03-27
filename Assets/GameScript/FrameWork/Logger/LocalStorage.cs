using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class LocalStorage
{

    public const string DEVICE_CODE_KEY = "DeviceCode";
    public const string GAME_VERSION = "GameVersion";

    public const string BGM_MUTE_KEY = "SETTINGPROPELLING_BGM";                                              //背景音乐
    public const string SOUND_MUTE_KEY = "SETTINGPROPELLING_SOUND";                                          //音效
    public const string BGM_MUTE_CHOOSE = "SETTINGMUTE_BGM";                                              //背景音乐
    public const string SOUND_MUTE_CHOOSE = "SETTINGMUTE_SOUND";                                          //音效
    public const string VIBRATE_MUTE_KEY = "SETTINGPROPELLING_VIBRATE";                                      //震动
    public const string CLEAR_LOG_COUNTER = "ClearFileLogCounter";          // 启动次数，连续N次启动之后，清理一次日志文件
    public const string CLEAR_BATTLELOG_COUNTER = "ClearBattleLogCounter";          // 启动次数，连续N次启动之后，清理一次日志文件



    #region 通用接口

    public static void SaveIntValue(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public static int GetIntValue(string key, int def = 0)
    {
        return PlayerPrefs.GetInt(key, def);
    }

    public static void SaveStringValue(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public static string GetStringValue(string key, string def = "")
    {
        return PlayerPrefs.GetString(key, def);
    }

    #endregion
}
