using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cfg.SLG;
namespace SunHeTBS
{
    public class Item
    {
        public static int itemSequence;
        public int sid;
        public int itemId;
        public int usedTimes = 0;
        public ItemData itemCfg;
        public Item(int itemId)
        {
            var cfg = ConfigManager.table.Item.Get(itemId);
            if (cfg != null)
            {
                itemCfg = cfg;
                itemSequence++;
                this.sid = itemSequence;
                usedTimes = 0;
                this.itemId = itemId;
            }
        }
    }
    public class Weapon : Item
    {
        public int forgeLevel;
        public int engraveId;
        public Weapon(int itemId) : base(itemId)
        {
            forgeLevel = 0;
            engraveId = 0;
            var cfg = ConfigManager.table.Item.Get(itemId);
            List<int> rangeCfg = cfg.Range;
            if (rangeCfg != null)
            {
                if (rangeCfg.Count == 1)
                {
                    this.rangeMin = rangeCfg[0];
                    this.rangeMax = rangeCfg[0];
                }
                else if (rangeCfg.Count == 2)
                {
                    this.rangeMin = rangeCfg[0];
                    this.rangeMax = rangeCfg[1];
                }
            }
        }
        public int rangeMax = 0;
        public int rangeMin = 0;

    }
}
