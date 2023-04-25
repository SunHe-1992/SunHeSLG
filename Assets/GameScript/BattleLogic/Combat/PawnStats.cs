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
        MAXVALUE = 7,
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
        public BasicAttribute(BasicAttribute a)
        {
            attr = new int[(int)BasicStats.MAXVALUE];
            AddAttr(a);
        }
        public BasicAttribute Clone(BasicAttribute a)
        {
            attr = new int[(int)BasicStats.MAXVALUE];
            AddAttr(a);
            return this;
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
            this.Str += config.Str;
            this.Mag += config.Mag;
            this.Dex += config.Dex;
            this.Res += config.Res;
            this.Luk += config.Luk;
            this.Bld += config.Bld;
        }
        public int GetAttr(BasicStats bsType)
        {
            return attr[(int)bsType];
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

        public void ApplyAttrCap(BasicAttribute cap)
        {
            for (int i = 0; i < attr.Length; i++)
            {
                if (attr[i] > cap.attr[i])
                {
                    attr[i] = cap.attr[i];
                }
            }
        }
        public void ApplyAttrFloor(BasicAttribute cap)
        {
            for (int i = 0; i < attr.Length; i++)
            {
                if (attr[i] < cap.attr[i])
                {
                    attr[i] = cap.attr[i];
                }
            }
        }
        public void AddAttr(BasicAttribute a)
        {
            for (int i = 0; i < attr.Length; i++)
            {
                attr[i] += a.attr[i];
            }
        }

    }

    public class CombatAttribute
    {
        public CombatAttribute()
        {

        }
        public int PhAtk;
        public int MagAtk;
        public int Hit;
        /// <summary>
        /// avoid foe's hit
        /// </summary>
        public int Avoid;
        public int CriticalRate;
        /// <summary>
        /// dodge foe's critical
        /// </summary>
        public int Dodge;
        public int AttackSpeed;

        public int StaffHit;
        public int StaffAvo;
        public int Defence;
        public int Resistance;

        public int DisplayedCrit;
        public int DisplayedHit;
        public int DisplayedStaffHit;
        public int DisplayedDamage;
        public void ReviseDisplayValues()
        {
            if (DisplayedDamage < 0)
                DisplayedDamage = 0;
            DisplayedCrit = ReviseRates(DisplayedCrit);
            DisplayedHit = ReviseRates(DisplayedHit);
            DisplayedStaffHit = ReviseRates(DisplayedStaffHit);
        }
        int ReviseRates(int value)
        {
            if (value > 100)
                value = 100;
            if (value < 0)
                value = 0;
            return value;
        }
    }
    public static class AttrCalculator
    {

    }
}
