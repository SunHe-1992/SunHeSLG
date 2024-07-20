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

    void CostGold(long cost)
    {
        TBSPlayer.UpdatePointAmount(PointEnum.Gold, -cost);
    }
}
