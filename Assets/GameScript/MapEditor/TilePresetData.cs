using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SunHeTBS;
/// <summary>
/// editor set this data,then convert to json
/// </summary>
public class TilePresetData : MonoBehaviour
{
    public TilePassType passType = TilePassType.Passable;
    /// <summary>
    /// for ground pawns, extra move price 
    /// </summary>
    public int extraMovePrice = 0;
    public EffectType effectType = EffectType.None;
    public string tileName = "Floor";
  
}

