using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TilePresetData : MonoBehaviour
{
    public enum EffectType : int
    {
        /// <summary>
        /// normal tile 
        /// </summary>
        None,
        /// <summary>
        /// +30 avo
        /// </summary>
        Avoid,
        /// <summary>
        /// heal/turn+10
        /// </summary>
        Healing,
        /// <summary>
        ///  +30 Avo, heal/turn+10
        /// </summary>
        Protection,
        /// <summary>
        /// Mov+2
        /// </summary>
        Frost,
    }
    public enum PassType : int
    {
        All,
        Impassable,
        FliersOnly,
    }
    public PassType passType = PassType.All;
    /// <summary>
    /// walk through cost move points
    /// </summary>
    public int moveCost = 1;
    public EffectType effectType = EffectType.None;
    public string tileName = "Floor";
  
}

