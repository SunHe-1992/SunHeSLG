using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Singleton;
using UniFramework.Event;
using System.Linq;

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

    public void RollDice()
    {
        diceValue = Random.Range(1, 7);
        //Debugger.Log($"roll dice value={diceValue}");
        lastTileIndex = currentTileIndex;
        PawnMoveForward(diceValue);
        mapCtrl.PlayDiceAnim(diceValue);
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
