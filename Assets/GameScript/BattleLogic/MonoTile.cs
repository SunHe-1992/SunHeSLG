using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cfg;
public class MonoTile
{
    public int Index = 0;
    public MapConfig cfg;
    public MonoTile(int index)
    {
        Index = index;
        //load config data
       
    }

    public bool isCorner = false;
    public void LoadConfigData()
    {
        cfg = ConfigManager.table.TbMapConfig.Get(Index);
    }
}
