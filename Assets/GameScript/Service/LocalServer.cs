using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Singleton;
using UniFramework.Event;
using System.Linq;
using SunHeTBS;
public class LocalServer : ISingleton
{
    public static LocalServer Inst { get; private set; }
    public void OnCreate(object createParam)
    {
    }

    public void OnDestroy()
    {
    }

    public void OnUpdate()
    {
    }

    public static void Init()
    {
        Inst = UniSingleton.CreateSingleton<LocalServer>();
    }

    public void RequestBuildingUpgrade(int buildingId)
    {
        var buildingData = MonoPlayer.buildingDic[buildingId];
        //spend money
        var cfg = ConfigManager.table.TbBuilding.Get(buildingId, buildingData.buildingLevel);
        CostGold(cfg.Price);
        buildingData.buildingLevel++;

        UniEvent.SendMessage(GameEventDefine.Building_Changed);

    }
    void CostGold(long cost)
    {
        MonoPlayer.UpdatePointAmount(PointEnum.Gold, -cost);
    }
}
