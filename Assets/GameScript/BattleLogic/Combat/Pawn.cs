using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cfg;
namespace SunHeTBS
{
    public abstract class RPGEntity
    {
        public virtual void Update()
        {

        }
    }
    public class Pawn : RPGEntity
    {
        public RPGSide side;
        public static int sequenceNum = 0;
        private int pawnId;
        public int seqId;
        PawnData pawnCfg;

        public PawnData PawnCfg { get => pawnCfg; private set => pawnCfg = value; }

        public NPCMark npcMark;
        public Pawn(NPCMark nMark)
        {
            sequenceNum++;
            seqId = sequenceNum;
            npcMark = nMark;
            npcMark._pawn = this;
            pawnId = npcMark.NPCId;

            Init();
        }

        public Pawn(int id, RPGSide side)
        {
            sequenceNum++;
            seqId = sequenceNum;
            pawnId = id;
            Init();
            this.side = side;
        }
        void Init()
        {
            PawnCfg = ConfigManager.table.TbPawn.Get(pawnId);

            if (npcMark != null)
            {
                RefreshNPCMark();
                this.npcMark.triggerDistance = PawnCfg.TriggerDistance;
            }

            InitSkillList();
        }

        void RefreshNPCMark()
        {
            npcMark.RefreshTxtName(this.PawnCfg.Name);
        }


        #region attributes

        BasicAttribute basicAttr;
        void InitAttrs()
        {
            //读取pawn基础配置的属性值
            var attrCfg = ConfigManager.table.TbAttr.Get(this.pawnCfg.BaseAttrId);
            basicAttr = new BasicAttribute(attrCfg.BaseAttr);
        }
        BasicAttribute totalAttr;
        public BasicAttribute GetAttr()
        {
            if (totalAttr == null)
            {
                CalculateTotalAttr();
            }
            return totalAttr;
        }
        void CalculateTotalAttr()
        {
            InitAttrs();
            totalAttr = new BasicAttribute();
            totalAttr.AddAttr(this.basicAttr);
            this.HP = totalAttr.HPMax;
            this.SP = totalAttr.SPMax;
        }
        #endregion
        #region HP 

        public int HP;
        public int SP;
        public int GetHPMax()
        {
            return this.GetAttr().HPMax;
        }
        #endregion


        #region skills
        List<RPGSkill> activeSkills = new List<RPGSkill>();
        RPGSkill normalAttack;
        void InitSkillList()
        {
            activeSkills = new List<RPGSkill>();
            //默认每个人有个技能普攻 101
            AddSkill(101);
        }
        void AddSkill(int sklId)
        {
            normalAttack = new RPGSkill(sklId, this);
            activeSkills.Add(normalAttack);
        }
        public void NormalAttack(Pawn target)
        {
            if (this.normalAttack != null)
            {
                normalAttack.StartCast(target);
            }
        }

        #endregion

        #region update
        public override void Update()
        {
            //drive the update
            base.Update();
            foreach(var skl in this.activeSkills)
            {
                skl.Update();
            }
        }
        #endregion
    }

    public enum RPGSide
    {
        Default,
        Player,
        Villian,
        NPC,
    }

}
