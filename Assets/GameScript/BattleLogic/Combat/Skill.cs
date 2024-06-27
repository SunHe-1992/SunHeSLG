using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cfg.SLG;
using cfg;
namespace SunHeTBS
{
    public abstract class SkillBase
    {
        public Pawn Caster;
        public virtual void Update()
        {

        }

    }
    public enum SkillPhase
    {
        Default = 0,
        BeforeCast = 1,
        Casting = 2,
        AfterCast = 3,
    }
    public class RPGSkill : SkillBase
    {
        public SkillData skillCfg;

        private Pawn targetPawn;

        SkillPhase phase = SkillPhase.Default;
        float timer = 0;
        public RPGSkill(int skillId, Pawn caster)
        {
            this.skillCfg = ConfigManager.table.Skill.Get(skillId);
            this.Caster = caster;
        }

        public void StartCast()
        {
            timer = 0;
            this.phase = SkillPhase.BeforeCast;

        }
        public void StartCast(Pawn target)
        {
            targetPawn = target;
            StartCast();
        }
        void SkillTakeEffect()
        {
            switch (this.skillCfg.SkillEffect)
            {
                case 101: ProcessDamageSkill(); break;
            }
        }
        void ProcessDamageSkill()
        {
            float dmg = CalculateDamage(targetPawn);
            Debugger.Log("造成伤害 " + dmg);
            string dmgStr = "-" + (int)dmg;
            if (this.Caster.IsPlayerSide())
                PopupTextControl.CreateTextFairyLeft(dmgStr, FontType.PAtk);
            else
                PopupTextControl.CreateTextFairyRight(dmgStr, FontType.PAtk);
            targetPawn.RecieveDamage(dmg, this.Caster);
        }

        float CalculateDamage(Pawn target)
        {
            float dmg = 0;
            var attrA = this.Caster.GetAttr();
            var attrB = target.GetAttr();

            dmg += (attrA.PAtk / attrB.PDef) * skillCfg.Multiplier;
            dmg += skillCfg.AddDmg;
            return dmg;
        }

        public override void Update()
        {
            base.Update();
            if (this.phase == SkillPhase.Default)
            {
                return;
            }

            if (this.phase == SkillPhase.BeforeCast)
                UpdateBeforeCast();
            else if (this.phase == SkillPhase.AfterCast)
                UpdateAfterCast();
        }
        void UpdateBeforeCast()
        {
            timer += Time.deltaTime;
            if (timer >= this.skillCfg.AnimTime1)
            {
                timer = 0;
                phase = SkillPhase.AfterCast;
                SkillTakeEffect();
            }
        }
        void UpdateAfterCast()
        {
            timer += Time.deltaTime;
            if (timer >= this.skillCfg.AnimTime2)
            {
                timer = 0;
                phase = SkillPhase.Default;
                OnSkillEnd();
            }
        }
        void OnSkillEnd()
        {
            this.Caster.ProcessAfterSkill();
        }
    }


}
