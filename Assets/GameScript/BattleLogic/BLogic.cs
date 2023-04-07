using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Singleton;
namespace SunHeTBS
{
    /// <summary>
    /// Battle Logic
    /// </summary>
    public class BLogic : ISingleton
    {
       
        //todo read data , deploy battle field

        float FixedLogicTime = 0;
        public bool Running;
        /// <summary>
        /// key = tile id, value = pawn ref
        /// </summary>
        public Dictionary<int, Pawn> mapPawnDic = new Dictionary<int, Pawn>();

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
            int tileId = p.TilePosId();

            if (!mapPawnDic.ContainsKey(tileId))
            {
                mapPawnDic[tileId] = p;
            }
            else
            {
                Debugger.LogError($"map tile {p.curPosition.ToString()} already contains pawn!");
            }
        }
        public void AddTestPawn()
        {
            Pawn p = new Pawn();
            p.camp = PawnCamp.Player;
            p.curPosition = new Vector3Int(0, 0, 0);
            p.modelName = "M_AA_001";
            p.Init();
            AddPawn(p);
        }
        #endregion

        public void ResetBattleState()
        {
            pawnList.Clear();
            mapPawnDic.Clear();
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

        #region cursor select pawn
        public Vector3Int cursorPos;
        public Pawn selectedPawn;

        public BLogic()
        {
        }

        /// <summary>
        /// input order try move cursor,
        /// </summary>
        /// <param name="xAdd"></param>
        /// <param name="yAdd"></param>
        public void CursorInputMove(int xAdd, int yAdd)
        {
            Vector3Int newPos = new Vector3Int(xAdd, yAdd) + cursorPos;
            newPos = TBSMapService.Inst.map.TrimPos_Border(newPos);
            cursorPos = newPos;
        }
        void CheckCursorPos()
        {
            int tileId = TBSMapService.Inst.GetTileId(cursorPos);
            if (mapPawnDic.ContainsKey(tileId))//todo cursor points a pawn,show info and move area
            {

            }
            else //todo no pawn in cursor pos ,show tile info
            {

            }
        }


        public static BLogic Inst { get; private set; }
        public static void Init()
        {
            Inst = UniSingleton.CreateSingleton<BLogic>();
        }
        public void OnCreate(object createParam)
        {
        }

        public void OnUpdate()
        {
        }

        public void OnDestroy()
        {
            Running = false;
            selectedPawn = null;
            ResetBattleState();
        }
        public void OnFixedUpdate()
        {
        }
        #endregion
    }
}
