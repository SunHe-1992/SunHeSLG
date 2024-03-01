using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoPlayer
{
    public static UserDetail UserDetail; //玩家的所有信息
    public static int diceFactor = 0;
    public static void SetUserDetail()
    {
        //init user detail info
        GenerateTestUserDetail();
    }

    //debug function
    static void GenerateTestUserDetail()
    {
        UserDetail = new UserDetail();
        UserDetail.IndexOnMap = 1;
        UserDetail.currentChapter = 1;
        UserDetail.diceCount = 12;
        UserDetail.points.Add(new UserPoint(PointEnum.Gold, 3323));
        UserDetail.points.Add(new UserPoint(PointEnum.Gem, 8888));

    }
}
public class UserDetail
{

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
    public List<BuildingDetail> buildings = new List<BuildingDetail>();

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

public class BuildingDetail
{
    public int buildingId;
    public int buildingLevel;
}
