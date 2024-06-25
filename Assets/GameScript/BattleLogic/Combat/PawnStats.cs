using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SunHeTBS;
using cfg.SLG;
namespace SunHeTBS
{


    public enum CombatStats : int
    {
        HPMax = 0,
        SPMax = 1,
        Patk = 2,
        Pdef = 3,
        Eatk = 4,
        Edef = 5,
        Accuracy = 6,
        Speed = 7,
        Critical = 8,
        Evasion = 9,
        MAXVALUE = Evasion + 1,
    }

    /// <summary>
    /// A set of CombatStats data
    /// </summary>
    public class BasicAttribute
    {
        int[] attr;

        public BasicAttribute()
        {
            attr = new int[(int)CombatStats.MAXVALUE];
        }
        public BasicAttribute(BasicAttribute a)
        {
            attr = new int[(int)CombatStats.MAXVALUE];
            AddAttr(a);
        }
        public BasicAttribute Clone(BasicAttribute a)
        {
            attr = new int[(int)CombatStats.MAXVALUE];
            AddAttr(a);
            return this;
        }
        public BasicAttribute(cfg.SLG.BasicStats config)
        {
            attr = new int[(int)CombatStats.MAXVALUE];
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
            this.SPMax += config.SPMax;
            this.PAtk += config.Patk;
            this.PDef += config.Pdef;
            this.EAtk += config.Eatk;
            this.EDef += config.Edef;
            this.Accuracy += config.Accuracy;
            this.Speed += config.Speed;
            this.Critical += config.Critical;
            this.Evasion += config.Evasion;
        }
        public int GetAttr(CombatStats bsType)
        {
            return attr[(int)bsType];
        }
        public int HPMax
        {
            get { return attr[(int)CombatStats.HPMax]; }
            set { attr[(int)CombatStats.HPMax] = value; }
        }
        public int SPMax
        {
            get { return attr[(int)CombatStats.SPMax]; }
            set { attr[(int)CombatStats.SPMax] = value; }
        }
        public int PAtk
        {
            get { return attr[(int)CombatStats.Patk]; }
            set { attr[(int)CombatStats.Patk] = value; }
        }
        public int PDef
        {
            get { return attr[(int)CombatStats.Pdef]; }
            set { attr[(int)CombatStats.Pdef] = value; }
        }
        public int EAtk
        {
            get { return attr[(int)CombatStats.Eatk]; }
            set { attr[(int)CombatStats.Eatk] = value; }
        }
        public int EDef
        {
            get { return attr[(int)CombatStats.Edef]; }
            set { attr[(int)CombatStats.Edef] = value; }
        }
        public int Accuracy
        {
            get { return attr[(int)CombatStats.Accuracy]; }
            set { attr[(int)CombatStats.Accuracy] = value; }
        }
        public int Speed
        {
            get { return attr[(int)CombatStats.Speed]; }
            set { attr[(int)CombatStats.Speed] = value; }
        }
        public int Critical
        {
            get { return attr[(int)CombatStats.Critical]; }
            set { attr[(int)CombatStats.Critical] = value; }
        }
        public int Evasion
        {
            get { return attr[(int)CombatStats.Evasion]; }
            set { attr[(int)CombatStats.Evasion] = value; }
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


    public static class AttrCalculator
    {


    }


}
