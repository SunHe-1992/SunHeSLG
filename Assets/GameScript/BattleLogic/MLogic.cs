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
        UniEvent.AddListener(GameEventDefine.PawnJumpStop, OnJumpStop);
    }

    public void OnDestroy()
    {
        UniEvent.RemoveListener(GameEventDefine.PawnJumpStop, OnJumpStop);

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

    public void RollDice(int _diceFactor, int givenDice = -1)
    {
        //random dice value
        if (givenDice >= 0 && givenDice <= 6)
        {
            diceValue = givenDice;
        }
        else
            diceValue = Random.Range(1, 7);
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
    }
    void OnJumpStop(IEventMessage msg)
    {
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
        Debugger.Log($"HandleTileEvents event id = {eventId}");
        switch (eventId)
        {
            case 100: HandleAddMoney(eventId, param1, param2, tileId); break;
            case 101: HandleCostMoney(eventId, param1, param2, tileId); break;
            case 102: HandleBankHeist(eventId, param1, param2, tileId); break;
            case 103: HandleSlogGame(eventId, param1, param2, tileId); break;
            case 104: break;
            case 105: break;

        }
    }
    void HandleAddMoney(int eventId, float param1, int param2, int tileId)
    {
        long amount = MonoPlayer.diceFactor * chapterCfg.PriceBase;
        MonoPlayer.UpdateGoldAmount(amount);
        //mapCtrl.SaveTileEventParamInTileCtrl(tileId, amount);
        UIService.Inst.ShowMoneyAnim(amount);
    }
    void HandleCostMoney(int eventId, float param1, int param2, int tileId)
    {
        long amount = MonoPlayer.diceFactor * chapterCfg.PriceBase;
        MonoPlayer.UpdateGoldAmount(-amount);
        //mapCtrl.SaveTileEventParamInTileCtrl(tileId, -amount);
        UIService.Inst.ShowMoneyAnim(-amount);
    }
    public void HandleBankHeist(int eventId, float param1, int param2, int tileId)
    {
        // set up datas for bank heists
        MonopolyService.Inst.SetUpBankHeistData();
        FUIManager.Inst.HideUI(FUIDef.FWindow.MonopolyMain);
        FUIManager.Inst.ShowUI<UIPage_BankHeist>(FUIDef.FWindow.BankHeist);

    }

    public void HandleSlogGame(int eventId, float param1, int param2, int tileId)
    {
        MonopolyService.Inst.SetUpSlotGameData();
        FUIManager.Inst.HideUI(FUIDef.FWindow.MonopolyMain);
        FUIManager.Inst.ShowUI<UIPage_SlotGame>(FUIDef.FWindow.SlotGame);
    }
    #endregion
}
