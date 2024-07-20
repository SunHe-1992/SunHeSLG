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
        InsertItem(100, 3);
        InsertItem(101, 55);

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
    public static void SpendGold(long changeValue)
    {
        UpdateGoldAmount(GetGoldAmount() - changeValue);
    }
    #endregion

    #region item management

    public static void InsertItem(int itemId, int itemCount)
    {
        var findItem = UserDetail.items.Find(p => p.itemId == itemId);
        if (findItem != null)
        {
            findItem.itemCount += itemCount;
        }
        else
        {
            UserDetail.items.Add(new UserItem(itemId, itemCount));
        }
        Debugger.Log($"item inserted itemId={itemId} count={itemCount}");
    }
    public static void RemoveItem(int itemId, int itemCount)
    {
        var findItem = UserDetail.items.Find(p => p.itemId == itemId);
        if (findItem != null)
        {
            if (findItem.itemCount >= itemCount)
            {
                findItem.itemCount -= itemCount;
                UserDetail.items.RemoveAll(p => p.itemCount <= 0);
            }
            else
            {
                Debugger.LogWarning("item remove: insufficient, item id: " + itemId);
            }
        }
        else
        {
            Debugger.LogWarning("failed to find item to remove, item id: " + itemId);
        }
    }
    #endregion

    #region quests management
    public static void GenerateBoardQuests()
    {
        foreach (var data in ConfigManager.table.TbQuest.DataList)
        {
            //data.DialogueID
            QuestEntry qe = new QuestEntry(data.ID);
            UserDetail.boardQuests.Add(data.ID, qe);
        }
    }

    public static void AcceptQuest(int questId)
    {
        var data = UserDetail.boardQuests[questId];
        UserDetail.boardQuests.Remove(questId);

        data.status = QuestEntry.QuestStatus.Accepted;
        UserDetail.myQuests.Add(questId, data);
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
    public Dictionary<int, QuestEntry> boardQuests = new Dictionary<int, QuestEntry>();
    public Dictionary<int, QuestEntry> myQuests = new Dictionary<int, QuestEntry>();
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

    public UserItem(int itemId, int itemCount)
    {
        this.itemId = itemId;
        this.itemCount = itemCount;
    }
}
public class QuestEntry
{
    public int questId;
    public int currentProgress;
    public int maxProgress;
    public QuestStatus status = QuestStatus.Onboard;

    public QuestEntry(int questId)
    {
        this.questId = questId;
        var cfg = ConfigManager.table.TbQuest.Get(this.questId);
        this.maxProgress = cfg.Param1;
        this.status = QuestStatus.Onboard;
    }

    public enum QuestStatus
    {
        Onboard = 0,
        Accepted = 1,
        Finished = 2,
    }
    public void UpdateProgress(int progress)
    {
        currentProgress = progress;
        //if (currentProgress >= maxProgress)
        //{
        //    status = QuestStatus.Finished;
        //}
    }
}
