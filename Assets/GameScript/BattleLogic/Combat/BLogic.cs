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
        /// <summary>
        /// game control state machine
        /// </summary>
        public static GameControlState GCState = GameControlState.Default;
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
        public List<Pawn> teamPawnList = new List<Pawn>();
        public List<Pawn> villianPawnList = new List<Pawn>();
        public Pawn HeroPawn;

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
            this.ClearBattleField();
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
        /// <summary>
        /// get current action pawn's side
        /// </summary>
        /// <returns></returns>
        public RPGSide GetCurrentSide()
        {
            return GetCurrentActionPawn().side;
        }
        void SortActionPawnList()
        {
            actionPawnList = new List<Pawn>();
            foreach (var p in teamPawnList)
            {
                if (p.dead == false)
                    actionPawnList.Add(p);
            }
            foreach (var p in villianPawnList)
            {
                if (p.dead == false)
                    actionPawnList.Add(p);
            }
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
        #region game play end check
        public CombatResult combatResult = CombatResult.Default;
        public void CheckCombatEnd()
        {
            bool allVillianDead = true;
            foreach (var p in villianPawnList)
            {
                if (p.dead == false)
                {
                    allVillianDead = false;
                    break;
                }
            }

            bool allPlayerDead = true;
            foreach (var p in teamPawnList)
            {
                if (p.dead == false)
                {
                    allPlayerDead = false;
                    break;
                }
            }
            //combat over: show combat end UI
            if (allVillianDead) //player win 
            {
                combatResult = CombatResult.Victory;
            }
            else if (allPlayerDead) //player lose
            {
                combatResult = CombatResult.Defeated;
            }

            if (allVillianDead || allPlayerDead)
            {
                DoCombatEnd();
            }
        }
        public void DoCombatEnd()
        {
            UIPage_CombatPanel.Instance.HideUI();
            FUIManager.Inst.ShowUI<UIPage_CombatEnd>(FUIDef.FWindow.CombatEndUI);

        }
        #endregion

        #region villian auto select

        public Pawn selectedPawn;
        public void AutoSelectVillian()
        {
            foreach (var p in villianPawnList)
            {
                if (p.dead == false)
                {
                    selectedPawn = p;
                }
            }
        }
        #endregion

        public void ClearBattleField()
        {
            this.villianPawnList.Clear();
            this.teamPawnList.Clear();
            this.actionPawnList.Clear();
            this.HeroPawn = null;
            this.actionPawnIndex = 0;
            this.selectedPawn = null;
            this.combatResult = CombatResult.Default;
        }

        #region Flee
        public bool AttemptToFlee()
        {
            return true;
        }
        public void DoCombatFlee()
        {
            this.combatResult = CombatResult.Flee;
            this.DoCombatEnd();
        }
        #endregion
    }

    public enum GameControlState
    {
        Default,
        ActionMenu,
        TargetPawnSelecting,
        CastingSkill,
        TurnSwitch,
    }
    public enum CombatResult
    {
        Default,
        Victory,
        Defeated,
        Flee,
    }
}
