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
        #endregion
    }

}
