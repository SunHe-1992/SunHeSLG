using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Singleton;
using UniFramework.Event;
namespace SunHeTBS
{
    public enum GamePlayState : int
    {
        Default = 0,
        /// <summary>
        /// pawn swap space
        /// </summary>
        BeforeBattle = 1,
        MovieTime = 2,
        DialoguePlay = 3,
        ActionPlay = 4,
        PhaseSwitch = 5,
        /// <summary>
        /// waiting for player controll a pawn
        /// </summary>
        SelectingPawn = 6,
        /// <summary>
        /// player select a move destination
        /// </summary>
        SelectingMoveDest = 7,
        /// <summary>
        /// waiting
        /// </summary>
        PawnMoving = 8,
        /// <summary>
        /// combat HUD, and pawn attack anim
        /// </summary>
        CombatPlay = 9,

        /* UI stats : for UI and map switch*/
        /// <summary>
        /// UI exp and level up
        /// </summary>
        UIExpPlay = 10,
        UIBattlePrepare = 11,
        /// <summary>
        /// pawn moves to dest,and show action menu UI
        /// </summary>
        UIActionMenu = 12,
        /// <summary>
        /// UI combat predict ,and target pawn select
        /// </summary>
        UICombatPredict = 13,

    }

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
        GamePlayState gameState = GamePlayState.Default;
        /// <summary>
        /// change to target state
        /// </summary>
        GamePlayState nextGameState = GamePlayState.Default;

        public PawnCamp curCamp { get; private set; }
        List<PawnCamp> campList;
        public PawnCamp GetNextCamp()
        {

            campList = new List<PawnCamp>();
            HashSet<PawnCamp> hash = new HashSet<PawnCamp>();
            foreach (var pawn in pawnList)
            {
                hash.Add(pawn.camp);
            }
            campList.AddRange(hash);
            campList.Sort();
            if (campList.Count == 0)
            {
                Debugger.LogError("no valid camp !");
            }
            PawnCamp camp = campList[0];
            if ((int)camp + 1 > campList.Count)
                camp = PawnCamp.Player;
            else
                camp = curCamp + 1;
            return camp;
        }
        public GamePlayState GetGamePlayState()
        {
            return gameState;
        }
        public void SetNextGamePlayState(GamePlayState gps)
        {
            nextGameState = gps;
        }
        bool SwitchBattleState()
        {
            //state changes
            if (nextGameState != GamePlayState.Default && gameState != nextGameState)
            {
                Debug.Log($"game play state {gameState}=>{nextGameState}");
                gameState = nextGameState;
                switch (nextGameState)
                {
                    case GamePlayState.BeforeBattle: OnEnterBeforeBattle(); break;
                    case GamePlayState.MovieTime: OnEnterMovieTime(); break;
                    case GamePlayState.ActionPlay: break;
                    case GamePlayState.DialoguePlay: break;
                    case GamePlayState.PhaseSwitch: OnEnterPhaseSwitch(); break;
                    case GamePlayState.SelectingPawn: OnEnterSelectPawn(); break;
                    case GamePlayState.SelectingMoveDest: OnEnterSelectingMoveDest(); break;
                    case GamePlayState.PawnMoving: break;
                    case GamePlayState.UIActionMenu: break;
                    case GamePlayState.UICombatPredict: break;
                    case GamePlayState.CombatPlay: break;
                    case GamePlayState.UIExpPlay: break;
                }
                return true;
            }
            //no change
            return false;
        }
        public void OnLogicUpdate(float dt)
        {
            FixedLogicTime += dt;
            LogicStateUpdate(dt);
        }

        void LogicStateUpdate(float dt)
        {
            if (!this.Running)
                return;

            if (SwitchBattleState())
            {
                return;
            }
            switch (gameState)
            {
                case GamePlayState.PawnMoving: PawnMovingUpdate(dt); break;
            }
        }
        #endregion

        #region BattleState: before battle

        void OnEnterBeforeBattle()
        {
            FUIManager.Inst.ShowUI<UIPage_BattlePrepare>(FUIDef.FWindow.BattlePrepare);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void StartPlayerPhase()
        {
            //nextGameState =  GamePlayState.;

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
                var tileEntity = TBSMapService.Inst.map.Tile(p.curPosition);
                tileEntity.camp = p.camp;
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
        public Vector3Int oldCursorPos;
        public Vector3Int cursorPos;
        /// <summary>
        /// pawn under cursor
        /// </summary>
        public Pawn pointedPawn;
        /// <summary>
        /// players pawn selected ,waiting for order
        /// </summary>
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
            oldCursorPos = cursorPos;
            Vector3Int newPos = new Vector3Int(xAdd, yAdd) + cursorPos;
            newPos = TBSMapService.Inst.map.TrimPos_Border(newPos);
            cursorPos = newPos;
            CheckCursorPos();
        }
        public void CursorInputMoveTo(Vector3Int pos)
        {
            var newPos = TBSMapService.Inst.map.TrimPos_Border(pos);
            cursorPos = newPos;
            oldCursorPos = newPos;
            CheckCursorPos();
        }
        void CheckCursorPos()
        {
            int tileId = TBSMapService.Inst.GetTileId(cursorPos);
            if (mapPawnDic.ContainsKey(tileId))//todo cursor points a pawn,show info and move area
            {
                pointedPawn = mapPawnDic[tileId];
            }
            else
            {
                pointedPawn = null;
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
            OnLogicUpdate(Time.deltaTime);

        }

        public void OnDestroy()
        {
            Running = false;
            pointedPawn = null;
            ResetBattleState();
        }
        public void OnFixedUpdate()
        {

        }
        #endregion

        public void OnEnterMovieTime()
        {
            //no movie in demo ,show battle main page
            FUIManager.Inst.ShowUI<UIPage_BattleMain>(FUIDef.FWindow.BattlePanel,
                (win) => { SetNextGamePlayState(GamePlayState.PhaseSwitch); });
        }
        public void OnEnterPhaseSwitch()
        {
            curCamp = GetNextCamp();
            UniEvent.SendMessage(GameEventDefine.PhaseSwitch);
        }
        public void PhaseSwitchDone()
        {
            SetNextGamePlayState(GamePlayState.SelectingPawn);
        }

        public void OnEnterSelectPawn()
        {
            UniEvent.SendMessage(GameEventDefine.ShowSelectPawn);
        }

        public void OnEnterSelectingMoveDest()
        {
            selectedPawn = pointedPawn;

        }

        #region pawn move
        Pawn movingPawn = null;
        public void PawnStartMove(Pawn p, int tileId)
        {
            movingPawn = p;
            var mapInst = GetMapInst();
            var toTile = mapInst.GetTileFromDic(tileId);
            var startTile = mapInst.Tile(movingPawn.curPosition);
            var nodeList = NodePathFinder.Path(mapInst, startTile, toTile, movingPawn.IsPassFoe());
            movingPawn.SetMovePath(nodeList);
            pawnMoveTime = movingPawn.moveTileTime * nodeList.Count;
            SetNextGamePlayState(GamePlayState.PawnMoving);
            movingPawn.StartMove();
        }
        float pawnMoveTime = 0;
        private void PawnMovingUpdate(float dt)
        {
            pawnMoveTime -= dt;
            if (pawnMoveTime < 0)
            {
                pawnMoveTime = 0;
                PawnMoveToTile();
            }
        }



        void PawnMoveToTile()
        {
            //movingPawn
            if (movingPawn != null)
            {
                Debugger.Log($" move end {movingPawn}");
                movingPawn.StopMove();
                if (movingPawn.InstantActionAfterMove)//todo act to target pawn : attack/heal/dance
                {
                    SetNextGamePlayState(GamePlayState.CombatPlay);
                }
                else
                {
                    SetNextGamePlayState(GamePlayState.UIActionMenu);
                    //show pawn's atk planes on this tile
                    TBSMapService.Inst.ShowPawnCoverPlanesOneTile(movingPawn, movingPawn.tempPos);
                    //show action menu
                    UniEvent.SendMessage(GameEventDefine.ShowActionMenu);
                }
                movingPawn = null;
            }
        }
        #endregion

        MapEntity GetMapInst()
        {
            return TBSMapService.Inst.map;
        }


    }
}
