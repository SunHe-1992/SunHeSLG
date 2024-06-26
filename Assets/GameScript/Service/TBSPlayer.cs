using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Event;
using SunHeTBS;
public static class TBSPlayer
{
    public static UserDetail UserDetail; //玩家的所有信息
    public static void SetUserDetail()
    {
        //init user detail info
        GenerateTestUserDetail();
        pointDic = new Dictionary<PointEnum, long>();
        foreach (var pt in UserDetail.points)
        {
            pointDic[(PointEnum)pt.PointType] = pt.PointValue;
        }
    }

    /// <summary>
    /// generate user detail info(should be delivered from server, or read from player's save file)
    /// </summary>
    static void GenerateTestUserDetail()
    {
        if (UserDetail != null)
            return;
        UserDetail = new UserDetail();
        UserDetail.userId = 301410195;
        UserDetail.userName = "He Sun";
        UserDetail.IndexOnMap = 39;
        UserDetail.currentChapter = 1001;
        UserDetail.diceCount = 12;
        UserDetail.points.Add(new UserPoint(PointEnum.Gold, 3323));
        UserDetail.points.Add(new UserPoint(PointEnum.Gem, 8888));
    }
    #region Point management

    static Dictionary<PointEnum, long> pointDic;
    public static long GetPointAmount(PointEnum ptType)
    {
        if (pointDic.ContainsKey(ptType))
            return pointDic[ptType];
        else
            return 0;
    }
    public static long GetGoldAmount()
    {
        return GetPointAmount(PointEnum.Gold);
    }
    public static bool IsAfford(PointEnum ptType, long needValue)
    {
        return GetPointAmount(ptType) >= needValue;
    }
    public static bool IsAffordGold(long needValue)
    {
        return GetPointAmount(PointEnum.Gold) >= needValue;
    }

    public static void UpdatePointAmount(PointEnum ptType, long changeValue)
    {
        if (pointDic.ContainsKey(ptType))
        {
            pointDic[ptType] += changeValue;
        }
        return;
    }
    public static void UpdateGoldAmount(long changeValue)
    {
        UpdatePointAmount(PointEnum.Gold, changeValue);
    }
    #endregion

 
}
public class UserDetail
{
    public int userId;
    public string userName;
    public int currentChapter = 0;
    /// <summary>
    /// current index on map
    /// </summary>
    public int IndexOnMap = 0;
    /// <summary>
    /// how many dices I have
    /// </summary>
    public int diceCount = 0;
    public List<UserPoint> points = new List<UserPoint>();
    public List<UserItem> items = new List<UserItem>();

}
public enum PointEnum
{
    Gold = 1,
    Gem = 2,
}
public class UserPoint
{
    public int PointType;
    public long PointValue;
    public UserPoint(PointEnum type, long value)
    {
        this.PointType = (int)type;
        this.PointValue = value;
    }
}
public class UserItem
{
    public int itemId;
    public int itemCount;
}
