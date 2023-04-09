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
    /// <summary>
    /// battle machine state
    /// </summary>
    public enum BattleState
    {
        Default = 0,
        BeforeBattle,
        PlayerPhase,
        /// <summary>
        /// player's enemy phase
        /// </summary>
        VillainPhase,
        PlayerAllyPhase,
        NeutralPhase,
        AfterBattle,
        Story,
        Ending,
    }

    public enum PawnCamp
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




}
