using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SunHeSLG
{
    public static class BattleManager
    {
        public static void StartLocalBattle()
        {
            // todo  generate  random seed number
    
            BattleDriver.Instance.SwitchDriveState(BattleDriveState.STATE_PREPARE_BATTLE);
        }
    }
}