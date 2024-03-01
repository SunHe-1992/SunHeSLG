using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Singleton;
using UniFramework.Event;
using System.Linq;
using SunHeTBS;
public class MLogic : ISingleton
{
    public static readonly int tileCount = 40;
    public static readonly int tileIndexMax = 39;
    MonopolyMapController mapCtrl;
    public static MLogic Inst;
    Dictionary<int, MonoTile> tileDic;
    public int lastTileIndex = 0;
    public int currentTileIndex = 0;
    public void OnCreate(object createParam)
    {
        tileDic = new Dictionary<int, MonoTile>();
        for (int i = 0; i < tileCount; i++)
        {
            tileDic[i] = new MonoTile(i);
        }
        lastTileIndex = 0;
        currentTileIndex = 0;
    }

    public void OnDestroy()
    {
    }

    public void OnUpdate()
    {
    }
    public void MapSceneLoaded()
    {
        mapCtrl = MonopolyMapController.inst;
    }

    public int diceValue = 0;

    public void RollDice(int diceFactor)
    {
        //random dice value
        diceValue = Random.Range(1, 7);
        MonoPlayer.UserDetail.diceCount -= diceFactor;
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

    public void PawnMoveForward(int step)
    {
        for (int i = 0; i < step; i++)
        {
            currentTileIndex++;
            if (currentTileIndex >= tileCount)
                currentTileIndex = 0;
        }
        //todo handle move to new tile with current Tile Index
    }
}
