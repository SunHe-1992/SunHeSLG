using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SunHeSLG
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
        Default,
        Player,
        /// <summary>
        /// 
        /// </summary>
        Villain,
        PlayerAlly,
        Neutral,
    }
}
