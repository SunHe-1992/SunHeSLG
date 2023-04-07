using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SunHeTBS
{
    public static class BattleManager
    {
        public static void StartLocalBattle()
        {
            // todo  generate  random seed number
    
            BattleDriver.Inst.SwitchDriveState(BattleDriveState.STATE_PREPARE_BATTLE);
        }
    }
}
