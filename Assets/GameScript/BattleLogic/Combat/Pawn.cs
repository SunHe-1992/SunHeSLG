using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cfg;
namespace SunHeTBS
{
    public class Pawn
    {
        public static int sequenceNum = 0;
        private int pawnId;
        public int seqId;
        PawnData pawnCfg;

        public PawnData PawnCfg { get => pawnCfg; private set => pawnCfg = value; }

        public NPCMark npcMark;
        public Pawn(NPCMark nMark)
        {
            seqId = 0;
            sequenceNum++;
            npcMark = nMark;
            npcMark._pawn = this;
            pawnId = npcMark.NPCId;

            Init();
        }

        public Pawn(int id)
        {
            seqId = 0;
            sequenceNum++;
            pawnId = id;
            Init();
        }
        void Init()
        {
            PawnCfg = ConfigManager.table.TbPawn.Get(pawnId);

            if (npcMark != null)
            {
                RefreshNPCMark();
                this.npcMark.triggerDistance = PawnCfg.TriggerDistance;
            }
        }

        void RefreshNPCMark()
        {
            npcMark.RefreshTxtName(this.PawnCfg.Name);
        }
    }
}
