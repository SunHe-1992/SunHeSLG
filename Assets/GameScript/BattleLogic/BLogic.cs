using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SunHeSLG
{
    /// <summary>
    /// Battle Logic
    /// </summary>
    public class BLogic : LogicSingleton<BLogic>
    {

        //todo read data , deploy battle field

        float FixedLogicTime = 0;
        public bool Running;


        #region battle machine state manage

        /// <summary>
        /// current battle machine state
        /// </summary>
        BattleState battleState = BattleState.Default;
        /// <summary>
        /// change to target state
        /// </summary>
        BattleState m_nextBattleState = BattleState.Default;

        public BattleState GetBattleState()
        {
            return battleState;
        }
        bool SwitchBattleState()
        {

            //state changes
            if (m_nextBattleState != BattleState.Default && battleState != m_nextBattleState)
            {
                switch (m_nextBattleState)
                {
                    case BattleState.BeforeBattle: OnEnterBeforeBattle(); break;
                    case BattleState.AfterBattle: break;
                    case BattleState.PlayerAllyPhase: break;
                    case BattleState.PlayerPhase: break;
                    case BattleState.NeutralPhase: break;
                    case BattleState.VillainPhase: break;
                    case BattleState.Story: break;
                    case BattleState.Ending: break;
                }
                return true;
            }
            //no change
            return false;
        }
        void OnLogicUpdate(float dt)
        {
            FixedLogicTime += dt;
            LogicStateUpdate(dt);
        }

        void LogicStateUpdate(float dt)
        {
            if (!this.Running)
                return;

        }
        #endregion

        #region BattleState: before battle

        void OnEnterBeforeBattle()
        {

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void StartPlayerPhase()
        {
            m_nextBattleState = BattleState.PlayerPhase;

        }

        #region Pawn manage

        public List<Pawn> pawnList = new List<Pawn>();

        public void AddPawn(Pawn p)
        {
            if (pawnList == null)
                pawnList = new List<Pawn>();

            pawnList.Add(p);
        }
        public void AddTestPawn()
        {
            Pawn p = new Pawn();
            p.camp = PawnCamp.Player;
            p.curPosition = new Vector3Int(0, 0, 0);
            p.modelName = "M_AA_001";
            p.Init();
        }
        #endregion

        public void ResetBattleState()
        {
            pawnList.Clear();
        }

        #region create map pawns 
        //
        public void SetMapThing()
        {
            //
            // SetMonsterInfo();
        }
        public void PostInitProcess()
        {
            //test
            AddTestPawn();

            InitPlayerPosition();

            SetBattleCameraBoundary();
        }
        void InitPlayerPosition()
        {
            //todo

        }
        void SetBattleCameraBoundary()
        {
            //todo
        }
        #endregion
    }
}
