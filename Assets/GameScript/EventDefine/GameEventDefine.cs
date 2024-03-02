using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SunHeTBS
{

    public static class GameEventDefine
    {
        static int GenID(int module, int id)
        {
            return module * 1000 + id;
        }

        #region TBS game events
        public static int CURSOR_MOVED = GenID(0, 1);//map cursor moved
        public static int ClickConfirm = GenID(0, 2);
        public static int ClickCancel = GenID(0, 3);
        public static int InputAxis = GenID(0, 4);
        public static int PhaseSwitch = GenID(0, 5);
        public static int ShowSelectPawn = GenID(0, 6);
        public static int ShowActionMenu = GenID(0, 7);
        public static int CursorPointToPawn = GenID(0, 8);
        public static int ShowWeaponSelectUI = GenID(0, 9);

        #endregion

        #region Monopoly game events
        public static int DICE_COUNT_CHANGED = GenID(1, 1);
        public static int POINTS_CHANGED = GenID(1, 2);
        public static int Building_Changed = GenID(1, 3);
        #endregion
    }

}
