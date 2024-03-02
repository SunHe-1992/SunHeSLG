using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cfg.SLG;
using cfg;
namespace SunHeTBS
{
    public abstract class SkillBase
    {
        public Pawn CasterPawn;

    }
    public class ActiveSkill : SkillBase
    {
        public SkillData skillCfg;

        private Pawn _targetRole;
        public Pawn TargetRole
        {
            get
            {
                return _targetRole;
            }

            set
            {
                _targetRole = value;
            }
        }
        StrikeInfo strikeInfo;
        public Vector3Int TargetPos;

        public ActiveSkill(int skillId, Pawn caster)
        {
            this.skillCfg = ConfigManager.table.Skill.Get(skillId);
            this.CasterPawn = caster;
        }

        public void StartCast()
        {

        }
        public void StartCast(Pawn target, StrikeInfo sInfo)
        {
            strikeInfo = sInfo;
            TargetRole = target;
            for (int i = 0; i < skillCfg.AttackCount; i++)
            {
                sInfo.attackerSid = CasterPawn.sequenceId;
                sInfo.defenderSid = TargetRole.sequenceId;
                if (HitCheck() == false)//miss
                {
                    strikeInfo.result = StrikeResult.Miss;
                    return;
                }
                else
                {
                    bool isCrit = CritCheck();
                    if (isCrit)
                        strikeInfo.result = StrikeResult.Critical;
                    else
                        strikeInfo.result = StrikeResult.Hit;
                    SkillTakeEffect(isCrit);
                }
                if (target.combat_interrupt || CasterPawn.combat_interrupt)
                {
                    break;
                }
            }
        }
        void SkillTakeEffect(bool isCrit)
        {
            if (isCrit)
                strikeInfo.result = StrikeResult.Critical;
            int casterAtk = this.CasterPawn.GetAttackValue();
            DamageType dmgType = this.CasterPawn.GetDamageType();

            CombatAttribute targetAttr = this.TargetRole.GetCombatAttr();
            int def = targetAttr.GetDefByDmgType(dmgType);
            int dmg = casterAtk - def;
            if (isCrit)
                dmg = dmg * 3;
            strikeInfo.defenderHPChange = -dmg;
            strikeInfo.attackerHP = CasterPawn.HP;
            strikeInfo.attackerHPMax = CasterPawn.GetAttribute().HPMax;
            strikeInfo.defenderHP = TargetRole.HP;
            strikeInfo.defenderHPMax = TargetRole.GetAttribute().HPMax;
            this.TargetRole.TakeDamage(dmg);
        }
        bool HitCheck()
        {
            //hit 
            var attr = this.CasterPawn.GetCombatAttr();
            int hitValue = 0;
            hitValue += attr.Hit;
            hitValue += CasterPawn.equippedWeapon.itemCfg.Hit;
            var targetAttr = TargetRole.GetCombatAttr();
            hitValue -= targetAttr.Avoid;
            if (hitValue <= 0)
                return false;
            if (hitValue >= 100)
                return true;
            return hitValue > RandUtil.RandInRange(0, 100);
        }

        bool CritCheck()
        {
            //crit
            var attr = this.CasterPawn.GetCombatAttr();
            int critValue = 0;
            critValue += attr.CriticalRate;
            critValue += CasterPawn.equippedWeapon.itemCfg.Critical;
            var targetAttr = TargetRole.GetCombatAttr();
            critValue -= targetAttr.Dodge;
            if (critValue <= 0)
                return false;
            if (critValue >= 100)
                return true;
            return critValue > RandUtil.RandInRange(0, 100);
        }
    }

}
