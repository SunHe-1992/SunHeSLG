using System;

public class TimeUtil
{
    public static long timeDiff = 0;

    static long serverTimeZone = 0;

    static long unixBaseMillis = new DateTime(1970, 1, 1, 0, 0, 0).ToFileTimeUtc() / 10000;

    static int refreshHour = 0;

    public static long GetUnixTimeStampMillis(DateTime d)
    {
        return (d.ToFileTime() / 10000) - unixBaseMillis;
    }

    public static long GetUnixTimeStamp(DateTime d)
    {
        return GetUnixTimeStampMillis(d) / 1000;
    }

    public static long GetMilliseconds()
    {
        return GetUnixTimeStampMillis(DateTime.Now);
    }

    public static long GetSystemTime()
    {
        long seconds = GetUnixTimeStamp(DateTime.Now);
        return seconds;
    }




    public static long GetServerTime()
    {
        refreshHour = 0;
        long diff = timeDiff;	// 服务器获取的本地时间差 
        long time = GetSystemTime() + diff;
        return time;
    }

    //获取对应的string的时间
    public static long GetServerTimeByString()
    {
        long diff = timeDiff;	// 服务器获取的本地时间差 
        long time = GetUnixTimeStamp(DateTime.Now) + diff;
        return time;
    }

    // 获取服务器时间与本地时间的差值
    public static long GetServerTimeDiff()
    {
        return timeDiff;
    }

    public static long GetServerTimeZone()
    {
        return serverTimeZone;
    }

    public static long getSecondsFromTimeStr(string date)
    {
        string[] parts = date.Split(':');
        if (parts.Length == 2)
        {
            return Convert.ToInt32(parts[0]) * 3600 + Convert.ToInt32(parts[1]) * 60;
        }
        else if (parts.Length == 3)
        {
            return Convert.ToInt32(parts[0]) * 3600 + Convert.ToInt32(parts[1]) * 60 + Convert.ToInt32(parts[2]) * 1;
        }
        return -1;
    }
    /// <summary>
    /// 字符串=> 时分秒
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static int[] getHMS(string date)
    {
        string[] parts = date.Split(':');
        int[] intList = { 0, 0, 0 };
        for (int i = 0; i < parts.Length; i++)
        {
            intList[i] = Convert.ToInt32(parts[i]);
        }
        return intList;
    }

    public static long getTodayStartTime()
    {
        //服务器在北京时区
        var dtNow = timestampToDateTime(GetServerTime());
        DateTime dt = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, refreshHour, 0, 0);

        return GetUnixTimeStamp(dt);
    }

    public static long GetRegisterTimezoneUtcOffset()
    {
        long ServerTimeZero = GetServerTimeZone();
        if (ServerTimeZero != 0)
        {
            return ServerTimeZero;
        }
        return 0;
    }

    public static long GetDiffToRegisterTimezone()
    {
        long nowUtcOffset = (long)TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalSeconds;
        /*if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))  GetUtcOffset会考虑夏令时，所以这里不用加了
        {
            nowUtcOffset = nowUtcOffset + 3600;
        }*/

        long diff = TimeUtil.GetRegisterTimezoneUtcOffset(); //- nowUtcOffset;
        return diff;
    }

    public static DateTime timestampToDateTime(long time)
    {
        if (time == 0)
        {
            time = TimeUtil.TimeToRegisterTimezone(0);
        }
        DateTime dateTime = ConvertIntDateTime(time);
        return dateTime;
    }



    public static long TimeToRegisterTimezone(long time)
    {
        long diff = TimeUtil.GetDiffToRegisterTimezone();
        if (time == 0)
        {
            time = TimeUtil.GetServerTime();
        }
        return time + diff;
    }

    public static long parseTriggerTimeStr(string date)
    {
        long pastTime = TimeUtil.getSecondsFromTimeStr(date);

        if (pastTime < 0)
        {
            Debugger.LogError("Invalid Time Str to Parse: " + date);
            return 0;
        }

        long todayZero = TimeUtil.getTodayStartTime();
        long diff = TimeUtil.GetDiffToRegisterTimezone();
        long triggerTime = todayZero + pastTime - diff;

        long curTime = TimeUtil.GetServerTime();
        if (triggerTime <= curTime)
        {
            triggerTime += 86400;
        }

        return triggerTime;
    }

    /// <summary>
    /// 获取今天xx点的时间戳
    /// </summary>
    /// <param name="tString"></param>
    /// <returns></returns>
    public static long parseTimeStringToTS(string tString)
    {
        int[] pastTime = TimeUtil.getHMS(tString);
        DateTime nowDt = GetUnixDateTime(GetServerTime());
        DateTime dt = new DateTime(nowDt.Year, nowDt.Month, nowDt.Day, pastTime[0], pastTime[1], pastTime[2]);
        return (int)TimeUtil.GetUnixTimeStamp(dt);
    }
    /// <summary>
    /// 时间戳转DateTime
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static System.DateTime ConvertIntDateTime(double d)
    {
        System.DateTime time = System.DateTime.MinValue;

        System.DateTime startTime = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        time = startTime.AddSeconds(d).ToLocalTime();
        //if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(time)) // 考虑夏令时
        //{
        //    time = time.AddSeconds(3600);
        //}
        return time;
    }

    public static string FormatTime(long timeSeconds, int section = 3, bool colon = false)
    {
        long timeDays = timeSeconds / (24 * 3600);
        if (timeDays > 0)
        {
            timeSeconds %= (24 * 3600);
        }

        long timeHours = timeSeconds / 3600;
        if (timeHours > 0)
        {
            timeSeconds %= 3600;
        }

        long timeMinutes = timeSeconds / 60;
        if (timeMinutes > 0)
        {
            timeSeconds %= 60;
        }

        // Debugger.LogError("timeDays" + timeDays + "," + timeHours + ",");
        return colon ? FormatTimeColon(timeDays, timeHours, timeMinutes, timeSeconds, section) : FormatTimeChar(timeDays, timeHours, timeMinutes, timeSeconds, section);
    }


    public static string FormatTimeColon(long timeDays, long timeHours, long timeMinutes, long timeSeconds, int section)
    {
        string timeString = "";

        if (section > 0)
        {
            timeString = timeString + string.Format("{0:D2}", timeSeconds);
            --section;
        }

        if (section > 0)
        {
            timeString = string.Format("{0:D2}", timeMinutes) + ":" + timeString;
            --section;
        }

        if (section > 0)
        {
            timeString = string.Format("{0:D2}", timeHours) + ":" + timeString;
            --section;
        }

        if (section > 0)
        {
            timeString = string.Format("{0:D2} ", timeDays) + ":" + timeString;
            --section;
        }

        return timeString;
    }

    public static string FormatTimeChar(long timeDays, long timeHours, long timeMinutes, long timeSeconds, int section)
    {
        string timeString = "";


        if (timeDays > 0 && section > 0)
        {
            timeString = timeString + string.Format("{0:D2}", timeDays) + Translator.getText("TXT-DAY");

            //return timeString;
        }

        if ((timeHours > 0 || timeDays > 0) && section > 0)
        {
            //如果是中文就不加空格，如果不是就加空格区分

            timeString = timeString + string.Format("{0:D2}", timeHours) + Translator.getText("TXT-HOUR");

            --section;
        }

        if ((timeMinutes > 0 || timeHours > 0) && timeDays < 1 && section > 0)
        {
            //如果是中文就不加空格，如果不是就加空格区分

            timeString = timeString + string.Format("{0:D2}", timeMinutes) + ":";


            --section;
        }

        if (section > 0 && timeDays < 1 && timeHours < 1)
        {

            timeString = timeString + string.Format("{0:D2}", timeSeconds); //+ Translator.GetStr("TXT-10202");

        }

        return timeString;
    }

    public static string FormatSecondToTime(long timeSeconds, int section = 3)
    {
        string timeString = "";
        long timeDays = timeSeconds / (24 * 3600);

        if (timeDays > 0 && section > 0)
        {
            timeSeconds %= (24 * 3600);
        }

        long timeHours = timeSeconds / 3600;
        if (timeHours > 0 && section > 0)
        {
            timeSeconds %= 3600;
        }


        long timeMinutes = timeSeconds / 60;
        if (timeMinutes > 0 && section > 0)
        {
            timeSeconds %= 60;
        }


        timeString = "";
        if (timeDays > 0)
        {
            //timeString = string.Format(Translator.GetStr("TXT-1143"), string.Format("{0:D2}", timeDays), string.Format("{0:D2}", timeHours), string.Format("{0:D2}", timeMinutes));
            timeString = timeDays + Translator.getText("TXT-DAY") + timeHours + Translator.getText("TXT-HOUR");
        }
        else if (timeHours > 0)
            timeString = timeHours + Translator.getText("TXT-HOUR") + timeMinutes + Translator.getText("TXT-MINIUTE");
        else if (timeMinutes > 0)
            timeString = timeMinutes + Translator.getText("TXT-MINIUTE") + timeSeconds + Translator.getText("TXT-SECOND");
        else
            timeString = timeSeconds + Translator.getText("TXT-SECOND");

        return timeString;
    }

    public static string FormatSecondToTime_showtwoType(long timeSeconds)
    {
        string timeString = "";
        //string currLang = LocalTranslator.Instance.CurrentLanguage;
        //如果是中文就不加空格，如果不是就加空格区分
        string space = "";
        //if (currLang == "zh-CN" || currLang == "zh-TW")
        //    space = " ";

        string txtDay = "TXT-DAY";
        string txtHour = "TXT-HOUR";
        string txtMiniute = "TXT-MINIUTE";
        //string txtSecond = "秒";
        //string txtWeek = "周";
        TimeSpan duration = new TimeSpan(0, 0, (int)timeSeconds);
        if (duration.Days >= 1)
        {
            timeString = $"{duration.Days}{space}{txtDay}";
            timeString = $"{timeString}{duration.Hours}{space}{txtHour}";
        }
        else
        {
            timeString = $"{timeString}{duration.Hours}{space}{txtHour}";
            timeString = $"{timeString}{duration.Minutes}{space}{txtMiniute}";
            //timeString = $"{timeString}{duration.Seconds}{space}{txtSecond}";
        }
        return timeString;
    }

    //显示挂机奖励这种不超过2或者40小时的显示的内容
    public static string FormatSecondToTimeForSomeTime(long timeSeconds, int MaxHours = 24)
    {
        string timeString = "";
        long timeDays = timeSeconds / (24 * 3600);


        if (timeDays > 0)
        {
            if (MaxHours > 24)
            {
                long timeHours = timeSeconds / 3600;
                timeSeconds %= 3600;
                long timeMinutes = timeSeconds / 60;
                timeSeconds %= 60;

                string hours = timeHours >= MaxHours ? (MaxHours - 1).ToString() : timeHours > 9 ? timeHours.ToString() : (0 + timeHours.ToString());
                string minutes = timeHours >= MaxHours ? 59.ToString() : timeMinutes > 9 ? timeMinutes.ToString() : (0 + timeMinutes.ToString());
                string seconds = timeHours >= MaxHours ? 59.ToString() : timeSeconds > 9 ? timeSeconds.ToString() : (0 + timeSeconds.ToString());
                timeString = string.Format("{0}:{1}:{2}", hours, minutes, seconds);
            }
            else
            {
                string hours = 23.ToString();
                string minutes = 59.ToString();
                string seconds = 59.ToString();
                timeString = string.Format("{0}:{1}:{2}", hours, minutes, seconds);
            }
        }
        else
        {
            long timeHours = timeSeconds / 3600;
            timeSeconds %= 3600;
            long timeMinutes = timeSeconds / 60;
            timeSeconds %= 60;

            string hours = timeHours >= MaxHours ? (MaxHours - 1).ToString() : timeHours > 9 ? timeHours.ToString() : (0 + timeHours.ToString());
            string minutes = timeHours >= MaxHours ? 59.ToString() : timeMinutes > 9 ? timeMinutes.ToString() : (0 + timeMinutes.ToString());
            string seconds = timeHours >= MaxHours ? 59.ToString() : timeSeconds > 9 ? timeSeconds.ToString() : (0 + timeSeconds.ToString());
            timeString = string.Format("{0}:{1}:{2}", hours, minutes, seconds);
        }

        return timeString;
    }

    //计数到毫秒
    public static string FormatSecondToTimeForSomeTimePrecise(long timeSeconds, int MaxHours = 24)
    {
        string timeString = "";
        long timeDays = timeSeconds / (24 * 3600);

        if (timeDays > 0)
        {
            if (MaxHours > 24)
            {
                long timeHours = timeSeconds / 3600;
                timeSeconds %= 3600;
                long timeMinutes = timeSeconds / 60;
                timeSeconds %= 60;

                string hours = timeHours >= MaxHours ? (MaxHours - 1).ToString() : string.Format("{0:D2}", timeHours);
                string minutes = timeHours >= MaxHours ? 59.ToString() : string.Format("{0:D2}", timeMinutes);
                string seconds = timeHours >= MaxHours ? 59.ToString() : string.Format("{0:D2}", timeSeconds);
                timeString = string.Format("{0}:{1}:{2}", hours, minutes, seconds);
            }
            else
            {
                timeString = "23:59:59";
            }
        }
        else
        {
            long timeHours = timeSeconds / 3600;
            if (timeHours > 0)  //大于小时
            {
                timeSeconds %= 3600;
                long timeMinutes = timeSeconds / 60;
                timeSeconds %= 60;

                string hours = timeHours >= MaxHours ? (MaxHours - 1).ToString() : string.Format("{0:D2}", timeHours);
                string minutes = timeHours >= MaxHours ? 59.ToString() : string.Format("{0:D2}", timeMinutes);
                string seconds = timeHours >= MaxHours ? 59.ToString() : string.Format("{0:D2}", timeSeconds);
                timeString = string.Format("{0}:{1}:{2}", hours, minutes, seconds);
            }
            else
            {
                timeSeconds %= 3600;
                long timeMinutes = timeSeconds / 60;
                timeSeconds %= 60;
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                string minutes = timeHours >= MaxHours ? 59.ToString() : string.Format("{0:D2}", timeMinutes);
                string seconds = timeHours >= MaxHours ? 59.ToString() : string.Format("{0:D2}", timeSeconds);
                long millisecondsNum = (1000 - (Convert.ToInt64(ts.TotalMilliseconds) % 1000));
                if (millisecondsNum == 1000)
                    millisecondsNum = 999;
                string milliseconds = string.Format("{0:D2}", millisecondsNum / 10);
                timeString = string.Format("{0}:{1}:{2}", minutes, seconds, milliseconds);
            }
        }

        return timeString;
    }

    public static string FormatTSLeftTime(long targetTS)
    {
        if (GetServerTime() >= targetTS)
            return "";

        long leftTime = targetTS - GetServerTime();
        return FormatSecondToTime_showtwoType(leftTime);
    }

    //获取今天的日期
    public static DayOfWeek GetDayOfWeek()
    {
        DateTime nowTime = ConvertIntDateTime(GetServerTime());

        return nowTime.DayOfWeek;
    }

    public static bool GetTimeAfterRefer()
    {
        DateTime serverDate = ConvertIntDateTime(TimeToRegisterTimezone(0));

        return GetTimeAfterRefer(serverDate);
    }

    public static bool GetTimeAfterRefer(DateTime time, int RefreshAddHour = -1)
    {
        if (RefreshAddHour == -1)
            RefreshAddHour = refreshHour;

        DateTime now = GetUnixDateTime(GetServerTime());
        DateTime referDate = new DateTime(now.Year, now.Month, now.Day, RefreshAddHour, 0, 0);
        return GetTimeAfterRefer(time, referDate);
    }

    //获取一个时间戳通过string的数据
    public static long GetTimeByString(string timeShow)
    {
        var timeSpilts = timeShow.Split('-');
        DateTime referDate = new DateTime(int.Parse(timeSpilts[0]), int.Parse(timeSpilts[1]), int.Parse(timeSpilts[2]), 0, 0, 0);
        return TimeUtil.GetUnixTimeStamp(referDate);
    }

    public static bool GetTimeAfterRefer(DateTime time, DateTime refer)
    {
        int referHour = refer.Hour;
        int checkHour = time.Hour;
        int checkMinute = time.Minute;
        int checkSecond = time.Second;

        if ((checkHour * 3600 + checkMinute * 60 + checkSecond) >= (referHour * 3600))
        {
            return true;
        }

        return false;
    }


    public static bool GetWeekTimeAfterRefer(DateTime time)
    {
        DateTime now = GetUnixDateTime(GetServerTime());
        DateTime referDate = new DateTime(now.Year, now.Month, now.Day, refreshHour, 0, 0);
        return GetWeekTimeAfterRefer(time, referDate);
    }

    public static bool GetWeekTimeAfterRefer(DateTime time, DateTime refer)
    {
        int referHour = refer.Hour;
        int checkHour = time.Hour;
        int checkMinute = time.Minute;
        int checkSecond = time.Second;

        if ((checkHour * 3600 + checkMinute * 60 + checkSecond) >= (referHour * 3600))
        {
            return true;
        }

        return false;
    }


    //每天刷新(RefreshAddHour如果填写5，就是是否到5点刷新)
    public static bool GetNeedRefresh(long time1, long time2, int RefreshAddHour = -1)
    {

        if (time1 == 0 || time2 == 0)
        {
            return true;
        }

        long serverTime1 = TimeToRegisterTimezone(time1);
        long serverTime2 = TimeToRegisterTimezone(time2);

        if (Math.Abs(serverTime1 - serverTime2) >= 86400)
        {
            return true;
        }

        DateTime serverDate1 = ConvertIntDateTime(serverTime1);
        DateTime serverDate2 = ConvertIntDateTime(serverTime2);

        bool isAfterRefer1 = GetTimeAfterRefer(serverDate1, RefreshAddHour);
        bool isAfterRefer2 = GetTimeAfterRefer(serverDate2, RefreshAddHour);

        if (serverDate1.Day == serverDate2.Day)
        {
            if (isAfterRefer1 != isAfterRefer2)
            {
                return true;
            }
        }
        else
        {
            if (isAfterRefer1 == isAfterRefer2)
            {
                return true;
            }
        }

        return false;
    }

    #region    现在时间是否超过5点
    //判断现在时间是否超过了5点
    public static bool NowTimeOverFive(long time1)
    {
        if (time1 == 0)
        {
            return true;
        }

        long serverTime1 = TimeToRegisterTimezone(time1);

        DateTime serverDate1 = ConvertIntDateTime(serverTime1);

        bool isAfterRefer1 = GetTimeAfterRefer(serverDate1);

        if (isAfterRefer1)
        {
            return true;
        }

        return false;
    }

    #endregion

    #region    每周一5点刷新
    //每周5点刷新
    public static bool GetWeekNeedRefresh(long time1, long time2)
    {
        if (time1 == 0 || time2 == 0)
        {
            return true;
        }

        long serverTime1 = TimeToRegisterTimezone(time1);
        long serverTime2 = TimeToRegisterTimezone(time2);

        if (Math.Abs(serverTime1 - serverTime2) >= 86400 * 7)
        {
            return true;
        }

        DateTime serverDate1 = ConvertIntDateTime(serverTime1);
        DateTime serverDate2 = ConvertIntDateTime(serverTime2);

        bool isAfterRefer1 = GetTimeAfterRefer(serverDate1);
        bool isAfterRefer2 = GetTimeAfterRefer(serverDate2);

        //不是同一天我先关心我们是否在同一周内
        int[] Day = new int[] { 7, 1, 2, 3, 4, 5, 6 };
        int serverDate1_dayorWeek = Day[Convert.ToInt16(serverDate1.DayOfWeek)];
        int serverDate2_dayorWeek = Day[Convert.ToInt16(serverDate2.DayOfWeek)];

        if (serverDate1.Day == serverDate2.Day)
        {
            if (isAfterRefer1 != isAfterRefer2 && serverDate1_dayorWeek == 1)
            {
                return true;
            }
        }
        else
        {
            if (serverDate1_dayorWeek > serverDate2_dayorWeek)
            {
                //只关心现在的时间，因为一个是上周，一个是当前周
                if (serverDate2_dayorWeek == 1 && isAfterRefer2)
                {
                    return true;
                }
            }
        }

        return false;
    }
    #endregion


    public static int GetMonthByTimeStamp(long time)
    {
        long serverTime = TimeToRegisterTimezone(time);
        DateTime serverDate = ConvertIntDateTime(serverTime);
        bool isAfterRefer = GetTimeAfterRefer(serverDate);
        if (!isAfterRefer)
        {
            serverDate = serverDate.AddDays(-1);
        }

        return serverDate.Month;
    }
    /// <summary>
    /// 从fromTs算第一天 toTs是第几天
    /// </summary>
    /// <param name="fromTs"></param>
    /// <param name="toTs"></param>
    /// <param name="refreshTime"></param>
    /// <returns></returns>
    //public static int GetPassDays(long fromTs, long toTs, int refreshTime)
    //{
    //    if (refreshTime == -1) refreshTime = refreshHour;
    //    //将第一个时间戳改为当天八点的时间戳
    //    if (toTs < fromTs) return 0;

    //    //先获取今天的刷新点时间
    //    var dtNow = timestampToDateTime(GetServerTime());
    //    DateTime dt = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, refreshTime, 0, 0);
    //    long todayStartTime = GetUnixTimeStamp(dt);
    //    int addDay = 0;
    //    //if (fromTs < todayStartTime && toTs >= todayStartTime)
    //    //    addDay = 1;
    //    DateTime fromDT = GetUnixDateTime(fromTs);
    //    DateTime toDT = GetUnixDateTime(toTs);
    //    TimeSpan passed = toDT - fromDT;
    //    int tsDay = passed.Days;
    //    return tsDay + 1;
    //}

    public static int GetPassDays(long fromTs, long toTs, int refreshTime)
    {
        if (refreshTime == -1) refreshTime = refreshHour;
        if (toTs < fromTs) return 0;
        //先获取起始日的刷新点时间
        var fromDT = timestampToDateTime(fromTs);
        DateTime dt = new DateTime(fromDT.Year, fromDT.Month, fromDT.Day, refreshTime, 0, 0);
        long fromDayStart = TimeUtil.GetUnixTimeStamp(dt);
        int addDay = 0;
        if (fromTs < fromDayStart)//从起始日起始时间之前计时 则多算1天
        {
            addDay = 1;
        }
        long fromTimeDiff = toTs - fromDayStart;//目标时间到起始日起始时间 有几个86400
        int fromDays = 0;
        if (fromTimeDiff > 0)
            fromDays = (int)(fromTimeDiff / 86400) + (fromTimeDiff % 86400 > 0 ? 1 : 0);
        return addDay + fromDays;
    }

    /// <summary>
    /// 过了多少自然天(同一天是0)
    /// </summary>
    /// <param name="fromTs"></param>
    /// <param name="toTs"></param>
    /// <returns></returns>
    public static int GetCalendarPassDays(long fromTs, long toTs)
    {
        DateTime fromDT = GetUnixDateTime(fromTs);
        DateTime toDT = GetUnixDateTime(toTs);
        fromDT = new DateTime(fromDT.Year, fromDT.Month, fromDT.Day, 0, 0, 0);
        toDT = new DateTime(toDT.Year, toDT.Month, toDT.Day, 0, 0, 0);
        return (int)((toDT - fromDT).TotalDays + 1.1);
    }

    //指定登录时间之后多久的时间戳显示
    public static long GetPassDaysTime(int passday, long fromTs, long toTs)
    {
        DateTime now = GetUnixDateTime(GetServerTime());
        //先获取
        DateTime dt = new DateTime(now.Year, now.Month, now.Day, refreshHour, 0, 0);
        dt = dt.AddDays(0);

        long todayStartTime = TimeUtil.GetUnixTimeStamp(dt);

        int days = 0;

        if (fromTs >= todayStartTime && toTs >= todayStartTime)
        {
            todayStartTime = todayStartTime + passday * 86400;
            return todayStartTime;
        }
        days = 0;
        if (toTs > todayStartTime)
        {
            days = 1;
        }
        var num = ((todayStartTime - fromTs) * 1.0) / 86400;
        var num_ceil = Math.Ceiling((decimal)num);
        days += (int)num_ceil;

        if (days > passday)
        {
            todayStartTime = todayStartTime - Math.Abs(days - passday) * 86400;
        }
        else
        {
            todayStartTime = todayStartTime + Math.Abs(days - passday) * 86400;
        }

        return todayStartTime;
    }


    /// <summary>
    /// 下周x y点时间戳
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static int GetNextWeekTS(int dayOfWeek, int time = 8)
    {
        DateTime dt = GetUnixDateTime(GetServerTime());
        DateTime nextDay = GetNextWeekday(dt, (DayOfWeek)dayOfWeek);
        DateTime day2 = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, time, 0, 0);
        if (day2 < dt)
        {
            day2 = day2.AddDays(7);
        }
        return (int)TimeUtil.GetUnixTimeStamp(day2);
    }
    public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
    {
        // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
        int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
        return start.AddDays(daysToAdd);
    }

    public static int GetNextMonthTS(int time = 8)
    {
        DateTime dt = GetUnixDateTime(GetServerTime());
        DateTime day2 = new DateTime(dt.Year, dt.Month, 1, time, 0, 0);
        DateTime day3 = day2.AddMonths(1);
        return (int)TimeUtil.GetUnixTimeStamp(day3);
    }

    /// <summary>
    /// dateTime=>时间戳
    /// </summary>
    /// <returns></returns>
    public static long GetCurrentTimeUnix(DateTime dt)
    {
        TimeSpan cha = (dt - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)));
        long t = (long)cha.TotalSeconds;
        return t;
    }

    //获取当前北京时间
    public static string GetCurrentTimeInBeijing()
    {
        return DateTime.Now.ToShortTimeString().ToString();
    }

    //返回指定小时的日期
    public static int GetHourTimsStamp(int time)
    {
        if (time == 24)
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time - 1, 59, 59);
            return (int)TimeUtil.GetUnixTimeStamp(dt);
        }
        else
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time, 0, 0);
            return (int)TimeUtil.GetUnixTimeStamp(dt);
        }
    }

    //返回指定下一个小时的日期
    public static int GetNextHourTimsStamp(int time)
    {
        if (time == 24)
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time - 1, 59, 59);
            return (int)TimeUtil.GetUnixTimeStamp(dt);
        }
        else
        {
            if (DateTime.Now.Hour >= time)
            {
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time, 0, 0);
                dt = dt.AddDays(1);
                return (int)TimeUtil.GetUnixTimeStamp(dt);
            }
            else
            {
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time, 0, 0);
                return (int)TimeUtil.GetUnixTimeStamp(dt);
            }
        }
    }

    public static int GetNextHourTimsStampbyServer(DateTime datatime, int time)
    {
        if (time == 24)
        {
            DateTime dt = new DateTime(datatime.Year, datatime.Month, datatime.Day, time - 1, 59, 59);
            return (int)TimeUtil.GetUnixTimeStamp(dt);
        }
        else
        {
            if (datatime.Hour >= time)
            {
                DateTime dt = new DateTime(datatime.Year, datatime.Month, datatime.Day, time, 0, 0);
                dt = dt.AddDays(1);
                return (int)TimeUtil.GetUnixTimeStamp(dt);
            }
            else
            {
                DateTime dt = new DateTime(datatime.Year, datatime.Month, datatime.Day, time, 0, 0);
                return (int)TimeUtil.GetUnixTimeStamp(dt);
            }
        }
    }

    // 时间戳转换为本地时间对象     
    public static DateTime GetUnixDateTime(long unix)
    {
        //long unix = 1500863191;
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        DateTime newTime = dtStart.AddSeconds(unix);
        return newTime;
    }
    public static string tsToString(long ts)
    {
        var dt = TimeUtil.timestampToDateTime(ts);
        return dt.ToString();
    }
    public static string tsToShortTime(long ts)
    {
        var dt = TimeUtil.timestampToDateTime(ts);
        return dt.ToShortTimeString();
    }
    /// <summary>
    /// 获取跨天时间
    /// </summary>
    /// <returns></returns>
    public static int GetRefreshHour()
    {
        return refreshHour;
    }

    //判斷今天是周几 0: 星期天
    public static int GetWeekDay()
    {
        DateTime now = GetUnixDateTime(GetServerTime());
        return (int)now.DayOfWeek;
    }

    public static long getTimesStartTime(long time)
    {
        DateTime dateTime = GetUnixDateTime(time);

        DateTime dt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, refreshHour, 0, 0);

        return GetUnixTimeStamp(dt);
    }

    public static string SecondsToString(int seconds, bool typeHour = false)
    {
        int hour = seconds / 3600;
        seconds = seconds % 3600;
        int minutes = seconds / 60;
        seconds = seconds % 60;

        if (hour > 0 || typeHour)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minutes, seconds);
        }

        return string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    //整数秒转换24小时制
    public static string SecondsToHours(int Seconds)
    {
        TimeSpan time = new TimeSpan(0, 0, Seconds);
        string hours = time.Hours >= 10 ? time.Hours.ToString() : string.Format("0{0}", time.Hours);
        string minutes = time.Minutes >= 10 ? time.Minutes.ToString() : string.Format("0{0}", time.Minutes);
        string seconds = time.Seconds >= 10 ? time.Seconds.ToString() : string.Format("0{0}", time.Seconds);
        return string.Format("{0}:{1}:{2}", hours, minutes, seconds);
    }


    // 当前时间是每个月的几号，考虑刷新时间
    public static int GetMonthDay()
    {
        long currTime = GetServerTime() - refreshHour * 3600;
        long serverTime = TimeToRegisterTimezone(currTime);

        DateTime dt = ConvertIntDateTime(serverTime);
        return dt.Day;
    }

    // 当前时间是每个月的几号，考虑刷新时间
    public static int GetMonthDayByFive()
    {
        long currTime = GetServerTime() - 5 * 3600;
        long serverTime = TimeToRegisterTimezone(currTime);

        DateTime dt = ConvertIntDateTime(serverTime);
        return dt.Day;
    }
}
