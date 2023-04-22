using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SunHeTBS;
using cfg.SLG;
namespace SunHeTBS
{
    /// <summary>
    ///  Basic Stats
    /// </summary>
    public enum BasicStats : int
    {
        STARTVALUE = 0,
        /// <summary>
        /// Hit Points Max
        /// </summary>
        HPMax = 0,
        /// <summary>
        /// Strength
        /// </summary>
        Str = 1,
        /// <summary>
        /// Magic
        /// </summary>
        Mag = 2,
        /// <summary>
        /// Dexterity
        /// </summary>
        Dex = 3,
        /// <summary>
        /// Speed
        /// </summary>
        Spd = 4,
        /// <summary>
        /// Defense
        /// </summary>
        Def = 5,
        /// <summary>
        /// Resistance
        /// </summary>
        Res = 6,
        /// <summary>
        /// Luck
        /// </summary>
        Luk = 7,
        /// <summary>
        /// Build
        /// </summary>
        Bld = 8,
        /// <summary>
        /// Movement
        /// </summary>
        Mov = 9,
        MAXVALUE = 10,
    }

    public enum CombatStats : int
    {
        /// <summary>
        /// Physical Attack
        /// </summary>
        PhAtk = 0,
        /// <summary>
        /// Magic Attack
        /// </summary>
        MagAtk = 1,
        /// <summary>
        /// Hit Chance
        /// </summary>
        Hit = 2,
        /// <summary>
        /// Hit Avoidance
        /// </summary>
        Avo = 3,
        /// <summary>
        /// Critical Chance
        /// </summary>
        Crit = 4,
        /// <summary>
        /// Critical Dodge
        /// </summary>
        Ddg = 5,
        /// <summary>
        /// Range
        /// </summary>
        Rng = 6,
    }

    /// <summary>
    /// A set of BasicStats data
    /// </summary>
    public class BasicAttribute
    {
        int[] attr;

        public BasicAttribute()
        {
            attr = new int[(int)BasicStats.MAXVALUE];
        }
        public BasicAttribute(cfg.SLG.BasicStats config)
        {
            attr = new int[(int)BasicStats.MAXVALUE];
            AddConfigAttr(config);
        }
        public void Reset()
        {
            for (int i = 0; i < attr.Length; i++)
            {
                attr[i] = 0;
            }
        }
        public void AddConfigAttr(cfg.SLG.BasicStats config)
        {
            this.HPMax += config.HPMax;
            this.Str = config.Str;
            this.Mag = config.Mag;
            this.Dex = config.Dex;
            this.Res = config.Res;
            this.Luk = config.Luk;
            this.Bld = config.Bld;
            this.Mov = config.Mov;
        }

        public int HPMax
        {
            get { return attr[(int)BasicStats.HPMax]; }
            set { attr[(int)BasicStats.HPMax] = value; }
        }
        public int Str
        {
            get { return attr[(int)BasicStats.Str]; }
            set { attr[(int)BasicStats.Str] = value; }
        }
        public int Mag
        {
            get { return attr[(int)BasicStats.Mag]; }
            set { attr[(int)BasicStats.Mag] = value; }
        }
        public int Dex
        {
            get { return attr[(int)BasicStats.Dex]; }
            set { attr[(int)BasicStats.Dex] = value; }
        }
        public int Spd
        {
            get { return attr[(int)BasicStats.Spd]; }
            set { attr[(int)BasicStats.Spd] = value; }
        }
        public int Def
        {
            get { return attr[(int)BasicStats.Def]; }
            set { attr[(int)BasicStats.Def] = value; }
        }
        public int Res
        {
            get { return attr[(int)BasicStats.Res]; }
            set { attr[(int)BasicStats.Res] = value; }
        }
        public int Luk
        {
            get { return attr[(int)BasicStats.Luk]; }
            set { attr[(int)BasicStats.Luk] = value; }
        }
        public int Bld
        {
            get { return attr[(int)BasicStats.Bld]; }
            set { attr[(int)BasicStats.Bld] = value; }
        }
        public int Mov
        {
            get { return attr[(int)BasicStats.Mov]; }
            set { attr[(int)BasicStats.Mov] = value; }
        }
    }

}
