using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SunHeTBS
{
    /// <summary>
    /// pawn state
    /// </summary>
    public enum PawnState
    {
        Default = 0,
        Idle = 1,
        /// <summary>
        /// under control of player
        /// </summary>
        WaitingOrder,
        Moving,
        /// <summary>
        /// waiting for the end of combat performance
        /// </summary>
        CombatAct,
        ActionDone,
    }


    public enum PawnCamp : int
    {
        Default = 0,
        Player = 1,
        Villain = 2,
        PlayerAlly = 3,
        Neutral = 4,
    }


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
    /// <summary>
    /// tile pass type
    /// </summary>
    public enum TilePassType : int
    {
        /// <summary>
        /// passable for all pawns
        /// </summary>
        Passable = 0,
        /// <summary>
        /// not passable
        /// </summary>
        Impassable = 1,
        /// <summary>
        ///  Fliers can pass,others can not
        /// </summary>
        FliersOnly = 2,
    }
    public enum PawnMoveType : int
    {
        Ground = 0,
        Flier = 1,
    }


    public static class PawnCampTool
    {
        static Dictionary<PawnCamp, bool[]> friendDic = new Dictionary<PawnCamp, bool[]>()
        {
            //if a tile is default ,it is empty
            /*                            defalut player villian ally   neutral */
            { PawnCamp.Player,    new bool[]{true, true , false, true , false} },
            { PawnCamp.Villain,   new bool[]{true, false, true, false, false } },
            { PawnCamp.PlayerAlly,new bool[]{true, true , false ,true , false } },
            { PawnCamp.Neutral,   new bool[]{true, false ,false, false, true } },
        };
        public static bool CampsHostile(PawnCamp campA, PawnCamp campB)
        {
            return friendDic[campA][(int)campB] == false;
        }
        public static bool CampsFriend(PawnCamp campA, PawnCamp campB)
        {
            return friendDic[campA][(int)campB] == false;
        }
    }

}
