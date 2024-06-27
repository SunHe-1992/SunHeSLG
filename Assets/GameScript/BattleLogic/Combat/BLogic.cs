using System.Collections;
using System.Collections.Generic;
using UniFramework.Singleton;
using UniFramework.Event;
using SunHeTBS;
using static LandMark;

namespace SunHeTBS
{
    public class BLogic : ISingleton
    {
        public static BLogic Inst;
        public static void Init()
        {
            Inst = UniSingleton.CreateSingleton<BLogic>();


        }
        public static LandMark recentLandMark;
        public static NPCMark recentNPCMark;
        public void OnCreate(object createParam)
        {
            UniEvent.AddListener(GameEventDefine.LandMarkTriggered, OnLandMarkTriggered);

        }

        public void OnDestroy()
        {
            UniEvent.RemoveListener(GameEventDefine.LandMarkTriggered, OnLandMarkTriggered);

        }

        public void OnUpdate()
        {
            foreach (var pawn in villianPawnList)
            {
                pawn.Update();
            }
            foreach (var pawn in teamPawnList)
            {
                pawn.Update();
            }
        }

        void OnLandMarkTriggered(IEventMessage msg)
        {


        }
        void AskFishing(IEventMessage msg)
        {

        }
        void AskSLotGame(IEventMessage msg)
        {

        }
        public void StartLandMarkMiniGame()
        {
            var type = recentLandMark.eventType;
            switch (type)
            {
                case LandMarkEventType.Fishing:
                    FUIManager.Inst.ShowUI<UIPage_Fishing>(FUIDef.FWindow.Fishing);
                    break;
                case LandMarkEventType.Slot:
                    MinigameService.Inst.SetUpSlotGameData();
                    FUIManager.Inst.ShowUI<UIPage_SlotGame>(FUIDef.FWindow.SlotGame);
                    break;
                case LandMarkEventType.Harvest:
                    MinigameService.Inst.SetUpBankHeistData();
                    FUIManager.Inst.ShowUI<UIPage_BankHeist>(FUIDef.FWindow.BankHeist);
                    break;
            }
        }


        #region Pawn management
        public List<Pawn> mapPawnList = new List<Pawn>();
        public List<Pawn> teamPawnList = new List<Pawn>();
        public List<Pawn> villianPawnList = new List<Pawn>();
        public Pawn HeroPawn;
        public void CreatePawn(NPCMark nm)
        {
            if (mapPawnList == null)
                mapPawnList = new List<Pawn>();
            mapPawnList.Add(new Pawn(nm));
        }

        public void InitHeroPawn()
        {
            HeroPawn = new Pawn(1, RPGSide.Player);
        }
        public void AddTestTeamPawns()
        {
            if (teamPawnList == null)
                teamPawnList = new List<Pawn>();
            for (int i = 1; i < 5; i++)
            {
                teamPawnList.Add(new Pawn(i, RPGSide.Player));
            }
        }
        public void AddVillianPawns()
        {
            if (villianPawnList == null)
                villianPawnList = new List<Pawn>();
            for (int i = 1; i < 5; i++)
            {
                villianPawnList.Add(new Pawn(i, RPGSide.Villian));
            }
        }
        #endregion
        public void StartCombat()
        {

            InitHeroPawn();
            AddTestTeamPawns();
            AddVillianPawns();
            StartTurn();
        }
        #region Combat turns management 对局管理

        public int currentTurn = 0;


        public void StartTurn()
        {
            currentTurn++;
            SortActionPawnList();
            actionPawnIndex = 0;
            UniEvent.SendMessage(GameEventDefine.TurnSwitch);
        }
        /// <summary>
        /// index for actionPawnList
        /// </summary>
        public int actionPawnIndex = 0;
        //sort 排序 本局所有pawn 按照speed 从高到低排序
        public List<Pawn> actionPawnList = new List<Pawn>();
        public Pawn GetCurrentActionPawn()
        {
            return actionPawnList[actionPawnIndex];
        }
        void SortActionPawnList()
        {
            actionPawnList = new List<Pawn>();
            actionPawnList.AddRange(teamPawnList);
            actionPawnList.AddRange(villianPawnList);
            actionPawnList.Sort(ActionPawnSorter);
        }
        int ActionPawnSorter(Pawn p1, Pawn p2)
        {
            if (p1.dead != p2.dead)
            {
                return p1.dead.CompareTo(p2.dead);
            }
            int spd1 = p1.GetAttr().Speed;
            int spd2 = p2.GetAttr().Speed;
            if (spd1 != spd2)
            {
                return spd2.CompareTo(spd1);
            }
            else //速度相同按照sid
            {
                return p1.seqId.CompareTo(p2.seqId);
            }
        }
        public void OnPawnActionEnd()
        {
            actionPawnIndex++;
            if (actionPawnIndex < actionPawnList.Count)
            {
                //next pawn start action
                UniEvent.SendMessage(GameEventDefine.NextActionPawn);
                var curPawn = GetCurrentActionPawn();
                Debugger.Log("curPawn is player side? " + curPawn.IsPlayerSide());
                if (curPawn.IsPlayerSide() == false) //AI do nothing and go on;
                {
                    OnPawnActionEnd();
                }
                UniEvent.SendMessage(GameEventDefine.NextActionPawn);
            }
            else //next turn
            {
                StartTurn();
            }
        }


        #endregion
    }
}
