using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Singleton;
using UniFramework.Event;
using System.Linq;
using SunHeTBS;
using cfg;
public class MLogic : ISingleton
{
    public static readonly int tileCount = 40;
    public static readonly int tileIndexMax = 39;
    MonopolyMapController mapCtrl;
    public static MLogic Inst;
    public Dictionary<int, MonoTile> tileDic;
    public int lastTileIndex = 0;
    public int currentTileIndex = 0;
    public cfg.Chapter chapterCfg;
    public void OnCreate(object createParam)
    {
        tileDic = new Dictionary<int, MonoTile>();
        for (int i = 0; i < tileCount; i++)
        {
            tileDic[i] = new MonoTile(i);
        }

    }

    public void OnDestroy()
    {
    }

    public void OnUpdate()
    {
    }
    public void MapSceneLoaded()
    {
        lastTileIndex = MonoPlayer.UserDetail.IndexOnMap;
        currentTileIndex = lastTileIndex;

        mapCtrl = MonopolyMapController.inst;

        foreach (var tile in tileDic.Values)
        {
            tile.LoadConfigData();
        }

        mapCtrl.RefreshTiles();

        int chapterId = MonoPlayer.UserDetail.currentChapter;
        chapterCfg = ConfigManager.table.TbChapter.Get(chapterId);
    }

    public int diceValue = 0;
    public int diceFactor = 0;

    public void RollDice(int _diceFactor)
    {
        diceFactor = _diceFactor;
        //random dice value
        diceValue = 1;//Random.Range(1, 7);
        MonoPlayer.UserDetail.diceCount -= _diceFactor;
        //Debugger.Log($"roll dice value={diceValue}");
        lastTileIndex = currentTileIndex;
        PawnMoveForward(diceValue);
        mapCtrl.PlayDiceAnim(diceValue);

        UniEvent.SendMessage(GameEventDefine.DICE_COUNT_CHANGED);
    }

    public static void Init()
    {
        Inst = UniSingleton.CreateSingleton<MLogic>();
    }
    public List<int> triggerTileIdList;
    public void PawnMoveForward(int step)
    {
        triggerTileIdList = new List<int>();
        for (int i = 0; i < step; i++)
        {
            currentTileIndex++;
            if (currentTileIndex >= tileCount)
                currentTileIndex = 0;
            var cfg = ConfigManager.table.TbMapConfig.Get(currentTileIndex);
            if (cfg.EventId != 0 && (cfg.TriggerOnPass || i == step - 1))
            {
                triggerTileIdList.Add(currentTileIndex);
            }
        }
        //handle move to new tile with current Tile Index
        HandleTileEvents();
    }
    #region tile events
    void HandleTileEvents()
    {
        foreach (int id in triggerTileIdList)
        {
            var cfg = ConfigManager.table.TbMapConfig.Get(id);
            if (cfg.EventId != 0)
            {
                HandleOneEvent(cfg.EventId, cfg.EventParam1, cfg.EventParam2, id);
            }
        }
    }
    void HandleOneEvent(int eventId, float param1, int param2, int tileId)
    {
        switch (eventId)
        {
            case 100: HandleAddMoney(eventId, param1, param2, tileId); break;
            case 101: HandleCostMoney(eventId, param1, param2, tileId); break;
            case 102: HandleBankHeist(eventId, param1, param2, tileId); break;
            case 103: HandleJail(eventId, param1, param2, tileId); break;
            case 104: break;
            case 105: HandleChanceCard(eventId, param1, param2, tileId); break;

        }
    }
    void HandleAddMoney(int eventId, float param1, int param2, int tileId)
    {
        long amount = diceFactor * chapterCfg.PriceBase;
        MonoPlayer.UpdateGoldAmount(amount);
        mapCtrl.SaveTileEventParamInTileCtrl(tileId, amount);
    }
    void HandleCostMoney(int eventId, float param1, int param2, int tileId)
    {
        long amount = diceFactor * chapterCfg.PriceBase;
        MonoPlayer.UpdateGoldAmount(-amount);
        mapCtrl.SaveTileEventParamInTileCtrl(tileId, -amount);
    }
    void HandleBankHeist(int eventId, float param1, int param2, int tileId)
    {
        //todo
    }
    void HandleJail(int eventId, float param1, int param2, int tileId)
    {
        //todo
    }
    void HandleChanceCard(int eventId, float param1, int param2, int tileId)
    {
        //todo
    }
    #endregion
}
